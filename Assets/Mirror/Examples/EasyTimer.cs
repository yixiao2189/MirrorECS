using System;
using System.Collections.Generic;
using System.Linq;

public class EasyTimer
{
	public EasyTimer(int capacity,System.Func<float> getTime)
	{
		if (capacity < 0 || capacity >= 32)
			throw new Exception("[BehaviorTimer] Out of Range");
		this.capacity = capacity;
		_cdRecords = new float[capacity];
		_cds = new float[capacity];
		_getTimeFunc = getTime;
	}

	protected readonly int capacity = 0;
	System.Func<float> _getTimeFunc = null;

	protected float[] _cdRecords = null;
	protected float[] _cds = null;

	public virtual bool IsInCD(int key, bool unInitAsCD = false)
	{
		if (_cds[key] <= 0 || _cdRecords[key] <= 0)
			return unInitAsCD;
		return _getTimeFunc() - _cdRecords[key] < _cds[key];
	}

	public virtual void Use(int key, float cdValue)
	{
		_cdRecords[key] = _getTimeFunc();
		_cds[key] = cdValue;
	}

	public virtual void ClearCD(int key)
	{
		_cdRecords[key] = 0;
	}

	public virtual void Reset()
	{
		Array.Clear(_cdRecords, 0, capacity);
		Array.Clear(_cds, 0, capacity);
	}

}
