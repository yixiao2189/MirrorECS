
using Mirror;
using System.Collections.Generic;
using UnityEngine;

namespace Noah.Battle
{
	public interface EventContainerItem
	{
		int frameId { get; }
	}

	public class EventContainer<T> : MonoBehaviour where T : EventContainerItem
	{
		public EventContainer(int maxCapacity,bool autoClearWhenPop , int outFrame = 5)
		{
			this.maxCapacity = maxCapacity;
			this.outFrame = outFrame;
			this.autoClearWhenPop = autoClearWhenPop;
			Reset();
		}

		public bool autoClearWhenPop = true;
		public int maxCapacity ;
		public int outFrame;

		public bool[] emptyTags;
		public int tagged;
		public List<T>[] container;

		public void Reset()
		{
			container = new List<T>[maxCapacity];
			emptyTags = new bool[maxCapacity];
			tagged = 0;
		}


		public void Clear() {
			for (int idx = 0; idx < emptyTags.Length; idx++) {
				if (emptyTags[idx])
				{
					GetList(idx).Clear();
					emptyTags[idx] = false;
					tagged = tagged ^ (1 << idx); 
				}
			}
		}


		//把几帧之前的事件清理出去
		public void ClearOutFrame(int curFrame) {
			foreach (var list in container) {
				if (list == null)
					continue;
				for (int i = 0; i < list.Count; i++) {
					if (curFrame - list[i].frameId <= outFrame) {
						break;
					}
					list.RemoveAt(i);
					i++;
				}
			}
		}	

		protected List<T> GetList(int i)
		{
			if (container[i] == null) {
				container[i] = new List<T>(10);
			}
			return container[i];
		}

		//是否含某事件
		public bool Contains(int @event)
		{
			return (tagged & @event) != 0;
		}

		//取出某事件
		public List<T> Pop(int @event)
		{
			if (!Contains(@event))
				return null;
			int idx = GetIdx(@event);
			var result = GetList(idx);
			if(autoClearWhenPop)
				tagged = tagged ^ @event;
			emptyTags[idx] = true;
			return result;
		}

		//获得事件列表
		public List<T> Get(int @event)
		{
			if (!Contains(@event))
				return null;
			int idx = GetIdx(@event);
			var result = GetList(idx);
			return result;
		}

		//压入事件
		public void Push(int @event, T result)
		{
			tagged |= @event;
			var idx = GetIdx(@event);
			if (emptyTags[idx]) {
				GetList(idx).Clear();
				emptyTags[idx] = false;
			}
			GetList(idx).Add(result);
		}

		//修改事件.该事件为单一条目
		public void ModifySingle(int @event, T result)
		{
			if (Contains(@event))
			{
				var list = Get((int)@event);
				if(list != null)
					list[0] = result;
				return;
			}
			Push(@event, result);
		}

		public bool TryGetSingleValue(int @event,out T result) {
			if (Contains(@event))
			{
				var list = Get(@event);
				if (list != null) {
					result = list[0];
					return true;
				}
			}
			result = default(T);
			return false;
		}

		protected int GetIdx(int physicsEvent)
		{
			int idx = (int)System.Math.Log(physicsEvent, 2);
			return idx;
		}
	}
}


