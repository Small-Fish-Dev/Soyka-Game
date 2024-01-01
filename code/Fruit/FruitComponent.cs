namespace Soyka;

public partial class FruitComponent : Component 
{
	[Property] public string Name => fruit.Name;
	[Property] public float Size => fruit.Size;

	private Fruit fruit = Fruit.All[0];
	public Fruit Fruit
	{
		get => fruit;
		set 
		{
			fruit = value;

			var obj = GameObject;
			if ( obj == null )
				return;

			obj.Name = fruit.Name;
			obj.Transform.Scale = fruit.Size / 8f;
			
			var texture = Texture.Load( FileSystem.Mounted, fruit.Texture );
			Plane.SceneObject.Batchable = false;
			Plane.SceneObject.Attributes.Set( "FruitTexture", texture );
		}
	}

	public ModelRenderer Plane { get; private set; }
}
