namespace Soyka;

public partial class FruitComponent : Component 
{
	[Property] public string Name => Fruit.Name;
	[Property] public float Size => Fruit.Size;

	private Fruit fruit = Soyka.Fruit.All[0];
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
			obj.Transform.Scale = Fruit.Size;
		}
	}
}
