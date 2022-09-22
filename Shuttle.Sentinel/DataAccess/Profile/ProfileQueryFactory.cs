using System;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Sentinel.Events.Profile.v1;

namespace Shuttle.Sentinel.DataAccess.Profile
{
    public class ProfileQueryFactory : IProfileQueryFactory
    {
        private readonly IDeactivationQueryFactory _deactivationQueryFactory;

        public ProfileQueryFactory(IDeactivationQueryFactory deactivationQueryFactory)
        {
            Guard.AgainstNull(deactivationQueryFactory, nameof(deactivationQueryFactory));

            _deactivationQueryFactory = deactivationQueryFactory;
        }

        public IQuery Register(Guid primitiveEventId, Registered domainEvent)
        {
            Guard.AgainstNull(domainEvent, nameof(domainEvent));

            var now = DateTime.UtcNow;

            return RawQuery.Create($@"
{_deactivationQueryFactory.Deactivate("Profile", primitiveEventId)}

insert into Profile
(
	SentinelId,
	EffectiveFromDate,
	EffectiveToDate,
	EMailAddress
)
values
(
	@SentinelId,
	@EffectiveFromDate,
	@EffectiveToDate,
	@EMailAddress
);
")
                .AddParameterValue(Columns.SentinelId, primitiveEventId)
                .AddParameterValue(Columns.EffectiveFromDate, now)
                .AddParameterValue(Columns.EffectiveToDate, DateTime.MaxValue)
                .AddParameterValue(Columns.EMailAddress, domainEvent.EMailAddress);
        }

        public IQuery Search(Query.Profile.Specification specification)
        {
            return RawQuery.Create(@"
select
	Id,
	SentinelId,
	EffectiveFromDate,
	EffectiveToDate,
	EMailAddress,
	DateActivated,
	PasswordResetToken,
	PasswordResetTokenDateRequested
from
	Profile
where
(
    @SentinelId is null
    or
	Id = @SentinelId
)
and
(
    isnull(@EMailAddress, '') = ''
    or
    EMailAddress = @EMailAddress
)
and
(
    @PasswordResetToken is null
    or
    PasswordResetToken = @PasswordResetToken
)
and
(
    @SecurityToken is null
    or
    SecurityToken = @SecurityToken
)
and
(
    @EffectiveDate is null
    or
    (
        EffectiveFromDate <= @EffectiveDate
        and
        (
            @EffectiveDate = @DateTimeMaxValue
            or
            EffectiveToDate > @EffectiveDate
        )
    )
)
")
                .AddParameterValue(Columns.SentinelId, specification.SentinelId)
                .AddParameterValue(Columns.EMailAddress, specification.EMailAddress)
                .AddParameterValue(Columns.EffectiveDate, specification.EffectiveDate)
                .AddParameterValue(Columns.DateTimeMaxValue, DateTime.MaxValue)
                .AddParameterValue(Columns.SecurityToken, specification.SecurityToken)
                .AddParameterValue(Columns.PasswordResetToken, specification.PasswordResetToken);
        }

        public IQuery PasswordResetToken(Guid primitiveEventId, PasswordResetRequested domainEvent)
        {
            return RawQuery.Create(@"
update
    Profile
set
    PasswordResetToken = @PasswordResetToken,
    PasswordResetTokenDateRequested = @PasswordResetTokenDateRequested
where
    SentinelId = @SentinelId
and
    EffectiveToDate = @EffectiveToDate 
")
                .AddParameterValue(Columns.SentinelId, primitiveEventId)
                .AddParameterValue(Columns.EffectiveToDate, DateTime.MaxValue)
                .AddParameterValue(Columns.PasswordResetToken, domainEvent.PasswordResetToken)
                .AddParameterValue(Columns.PasswordResetTokenDateRequested, domainEvent.DateRequested);
        }

        public IQuery RegisterSecurityToken(string emailAddress, Guid securityToken)
        {
            Guard.AgainstNullOrEmptyString(emailAddress, nameof(emailAddress));

            return RawQuery.Create(@"
update
    Profile
set
    SecurityToken = @SecurityToken
where
    EMailAddress = @EMailAddress
and
    EffectiveToDate = @EffectiveToDate 
")
                .AddParameterValue(Columns.EMailAddress, emailAddress)
                .AddParameterValue(Columns.SecurityToken, securityToken)
                .AddParameterValue(Columns.EffectiveToDate, DateTime.MaxValue);
        }

        public IQuery RemoveSecurityToken(Guid securityToken)
        {
            return RawQuery.Create(@"
update
    Profile
set
    SecurityToken = null
where
    SecurityToken = @SecurityToken
")
                .AddParameterValue(Columns.SecurityToken, securityToken)
                .AddParameterValue(Columns.EffectiveToDate, DateTime.MaxValue);
        }
    }
}