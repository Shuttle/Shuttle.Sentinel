/*
    This file forms part of Shuttle.Sentinel.

    Shuttle.Sentinel - A management and monitoring solution for shuttle-esb implementations. 
    Copyright (C) 2016  Eben Roux

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel
{
	public class SystemUserColumns
	{
		public static readonly MappedColumn<Guid> Id = new MappedColumn<Guid>("Id", DbType.Guid);
		public static readonly MappedColumn<string> Username = new MappedColumn<string>("Username", DbType.String, 65);
		public static readonly MappedColumn<byte[]> PasswordHash = new MappedColumn<byte[]>("PasswordHash", DbType.Binary, 64);
		public static readonly MappedColumn<DateTime> DateRegistered = new MappedColumn<DateTime>("DateRegistered", DbType.DateTime);
		public static readonly MappedColumn<string> RegisteredBy = new MappedColumn<string>("RegisteredBy", DbType.String, 65);
		public static readonly MappedColumn<DateTime> DateActivated = new MappedColumn<DateTime>("DateActivated", DbType.DateTime);
	}
}