/*
	FluorineFx open source library 
	Copyright (C) 2007 Zoltan Csibi, zoltan@TheSilentGroup.com, FluorineFx.com 
	
	This library is free software; you can redistribute it and/or
	modify it under the terms of the GNU Lesser General Public
	License as published by the Free Software Foundation; either
	version 2.1 of the License, or (at your option) any later version.
	
	This library is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
	Lesser General Public License for more details.
	
	You should have received a copy of the GNU Lesser General Public
	License along with this library; if not, write to the Free Software
	Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*/
using System;
using System.Threading;

namespace FluorineFx.Util
{
    /// <summary>
    /// A long value that may be updated atomically.
    /// </summary>
    public class AtomicLong
    {
        long _counter;

        /// <summary>
        /// Initializes a new instance of the AtomicLong class with initial value 0.
        /// </summary>
        public AtomicLong()
            : this(0)
        {
        }
        /// <summary>
        /// Initializes a new instance of the AtomicLong class with the given initial value.
        /// </summary>
        /// <param name="initialValue">The initial value.</param>
        public AtomicLong(long initialValue)
        {
            _counter = initialValue;
        }
        /// <summary>
        /// Gets or sets the current value.
        /// </summary>
        public long Value
        {
            get
            {
                return _counter;
            }
            set
            {
                Interlocked.Exchange(ref _counter, value);
            }
        }
        /// <summary>
        /// Atomically increment by one the current value.
        /// </summary>
        /// <returns>The updated value.</returns>
        public long Increment()
        {
            return Interlocked.Increment(ref _counter);
        }
        /// <summary>
        /// Atomically decrement by one the current value.
        /// </summary>
        /// <returns>The updated value.</returns>
        public long Decrement()
        {
            return Interlocked.Decrement(ref _counter);
        }
        /// <summary>
        /// Atomically decrement by one the current value.
        /// </summary>
        /// <returns>The previous value.</returns>
        public long PostDecrement()
        {
            return Interlocked.Decrement(ref _counter) + 1;
        }
        /// <summary>
        /// Atomically increment by one the current value.
        /// </summary>
        /// <returns>The previous value.</returns>
        public long PostIncrement()
        {
            return Interlocked.Increment(ref _counter) - 1;
        }

#if NET_1_1
#else
        /// <summary>
        /// Atomically add the given value to current value.
        /// </summary>
        /// <param name="delta">The value to add.</param>
        /// <returns>The updated value.</returns>
        public long Increment(long delta)
        {
            return Interlocked.Add(ref _counter, delta);
        }
        /// <summary>
        /// Atomically decrement by given value the current value. 
        /// </summary>
        /// <param name="delta">The value to subtract.</param>
        /// <returns>The updated value.</returns>
        public long Decrement(long delta)
        {
            return Interlocked.Add(ref _counter, -delta);
        }
        /// <summary>
        /// Atomically add the given value to current value.
        /// </summary>
        /// <param name="delta">The value to add.</param>
        /// <returns>The previous value.</returns>
        public long PostIncrement(long delta)
        {
            return (Interlocked.Add(ref _counter, delta) - delta);
        }
        /// <summary>
        /// Atomically decrement by given value the current value.
        /// </summary>
        /// <param name="delta">The value to subtract.</param>
        /// <returns>The previous value.</returns>
        public long PostDecrement(long delta)
        {
            return Interlocked.Add(ref _counter, -delta) + delta;
        }
#endif
        /// <summary>
        /// Compares the current value with comparand for equality and, if they are equal, replaces the current value.
        /// </summary>
        /// <param name="value">The value that replaces the current value if the comparison results in equality.</param>
        /// <param name="comparand">The value that is compared to the current value.</param>
        /// <returns>The original value.</returns>
        public long CompareExchange(long value, long comparand)
        {
            return Interlocked.CompareExchange(ref _counter, value, comparand);
        }
        /// <summary>
        /// Atomically sets the value to the given updated value if the current value == the expected value.
        /// </summary>
        /// <param name="expect">The expected value.</param>
        /// <param name="update">The new value.</param>
        /// <returns>True if successful. False return indicates that the actual value was not equal to the expected.</returns>
        public bool CompareAndSet(long expect, long update)
        {
            return expect == Interlocked.CompareExchange(ref _counter, update, expect);
        }
        /// <summary>
        /// Sets the specified value as the current value and returns the original value, as an atomic operation.
        /// </summary>
        /// <param name="value">The value to which the current value is set.</param>
        /// <returns>The original value.</returns>
        public long Exchange(long value)
        {
            return Interlocked.Exchange(ref _counter, value);
        }
        /// <summary>
        /// Returns a string that represents the current AtomicLong object.
        /// </summary>
        /// <returns>A string that represents the current AtomicLong object.</returns>
        public override string ToString()
        {
            return this.Value.ToString();
        }
    }
}
