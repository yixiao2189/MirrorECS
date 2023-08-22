// MovementSystem.cs
using UnityEngine;
using UnityEngine.Profiling;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;
using System.Linq;
// MovementSystem updates any GameObject with a Transform & Movement Component
public class MovementSystem : EgoSystem<
    EgoParentConstraint<Transform, Movement,Server, EgoParentConstraint<Transform,EgoConstraint<Renderer>>>
>
{
    Bounds bounds = new Bounds(Vector3.zero,Vector3.one*32);

    public override void Start()
    {
        var goTplt =  GameObject.Find("template");
        // Create a Cube GameObject
        for (int i = 0; i < 10;i++)
        {
            var go = GameObject.Instantiate(goTplt);
            var cubeEgoComponent = Ego.AddGameObject(go);
            cubeEgoComponent.gameObject.name = $"{i}";
            cubeEgoComponent.transform.position = Vector3.zero;

            // Add a Movement Component to the Cube
            var moveComp = Ego.AddComponent<Movement>(cubeEgoComponent);
            moveComp.velocity = (Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)) * Vector3.up).normalized;

            if (i % 2 == 0)
                Ego.AddComponent<Server>(cubeEgoComponent);
        }

        EgoEvents<TriggerEnterEvent>.AddHandler(Handle);

    }

    const string FOREACH = "ForEach";

    public override void Update()
    {
        Profiler.BeginSample(FOREACH); 
        // For each GameObject that fits the constraint...
        constraint.ForEachGameObject((egoComponent, transform, movement,serverComp, holder) =>
        {
            /*
            var movement = transform.GetComponentInParent<Movement>();
            if (movement == null)
                return; 
            */
            transform.Translate(movement.velocity * Time.deltaTime);

            if(!bounds.Contains(transform.position))
                Ego.DestroyGameObject(egoComponent);


            var midEgoComp = constraint.childBundles[egoComponent].Keys.First();//查子级，一级查到二级
            var finalBundle = holder.childBundles[midEgoComp].First();//查子级，二级查三级
            var finalComp = (finalBundle.Value as EgoBundle<MeshFilter>).component1;
            Debug.Log(finalComp.GetComponent<Renderer>());
            
        });
        Profiler.EndSample();
    }

    public  void Handle(TriggerEnterEvent e)
    {
        
        var comp =   e.egoComponent1.GetComponent<Movement>();
      
        comp.velocity = -comp.velocity;
   

    }
}