using UnityEngine;
using System.Collections.Generic;


namespace Noah.Battle {
    public enum PhysicsEvent
    {
        Enter = 1,
        Stay = 2,
        Exit = 4,
    }

    public class TriggerEnterEvent :  EventContainerItem
    {
        public readonly EgoComponent egoComponent1;
        public readonly Collider collider;

        public TriggerEnterEvent(int frameID,EgoComponent egoComponent1, Collider collider)
        {
            this.egoComponent1 = egoComponent1;

            this.collider = collider;
            this.frameId = frameID;
        }

        public int frameId { get;  }
    }

    //Phbody默认只有关注PhysicsEvent.TriggerEnter. 联合查找,各自系统处理 eg:<PhCollision,Tower>
    public class PhCollisionContainer : EventContainer<TriggerEnterEvent>,IEgoComponent
	{
		public const int OUT_FRAME = 5;
		public PhCollisionContainer() : base(3, false,OUT_FRAME) { }


		//是否含某事件
		public bool Contains(PhysicsEvent @event)
		{
			return base.Contains((int)@event);
		}

		//取出某事件
		public List<TriggerEnterEvent> Pop(PhysicsEvent @event)
		{
			return base.Pop((int)@event);
		}

		public void Push(PhysicsEvent @event, TriggerEnterEvent result) {
			base.Push((int)@event,result);
		}

        EgoComponent egoComponent;

        void Awake()
        {
            egoComponent = GetComponent<EgoComponent>();
        }

        void OnTriggerEnter(Collider collider)
        {
            var e = new TriggerEnterEvent(Time.frameCount,egoComponent,collider);
            Push(PhysicsEvent.Enter,e);
        }
    }
}

