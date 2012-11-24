/*   
NotMissing
Copyright (C) 2011 The NotMissing Team

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

namespace System
{
    //TODO: document
    public delegate void OpenAction<in TThis>(TThis @this);
    public delegate void OpenAction<in TThis, in T>(TThis @this, T arg);
    public delegate void OpenAction<in TThis, in T, in T2>(TThis @this, T arg, T2 arg2);
    public delegate void OpenAction<in TThis, in T, in T2, in T3>(TThis @this, T arg, T2 arg2, T3 arg3);
    public delegate void OpenAction<in TThis, in T, in T2, in T3, in T4>(TThis @this, T arg, T2 arg2, T3 arg3, T4 arg4);
    public delegate void OpenAction<in TThis, in T, in T2, in T3, in T4, in T5>(TThis @this, T arg, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
    public delegate void OpenAction<in TThis, in T, in T2, in T3, in T4, in T5, in T6>(TThis @this, T arg, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);
    public delegate void OpenAction<in TThis, in T, in T2, in T3, in T4, in T5, in T6, in T7>(TThis @this, T arg, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);
    public delegate void OpenAction<in TThis, in T, in T2, in T3, in T4, in T5, in T6, in T7, in T8>(TThis @this, T arg, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);

    public delegate R OpenFunc<in TThis, out R>(TThis @this);
    public delegate R OpenFunc<in TThis, in T, out R>(TThis @this, T arg);
    public delegate R OpenFunc<in TThis, in T, in T2, out R>(TThis @this, T arg, T2 arg2);
    public delegate R OpenFunc<in TThis, in T, in T2, in T3, out R>(TThis @this, T arg, T2 arg2, T3 arg3);
    public delegate R OpenFunc<in TThis, in T, in T2, in T3, in T4, out R>(TThis @this, T arg, T2 arg2, T3 arg3, T4 arg4);
    public delegate R OpenFunc<in TThis, in T, in T2, in T3, in T4, in T5, out R>(TThis @this, T arg, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
    public delegate R OpenFunc<in TThis, in T, in T2, in T3, in T4, in T5, in T6, out R>(TThis @this, T arg, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);
    public delegate R OpenFunc<in TThis, in T, in T2, in T3, in T4, in T5, in T6, in T7, out R>(TThis @this, T arg, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);
    public delegate R OpenFunc<in TThis, in T, in T2, in T3, in T4, in T5, in T6, in T7, in T8, out R>(TThis @this, T arg, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);
}