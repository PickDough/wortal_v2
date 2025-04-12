using Godot;

namespace wortal_v2.scenes.runes;


public enum RuneStateEnum
{
	Invalid,
	NotPlaced,
	Overlapping,
	Placed,
	AimedAt
}

public abstract record RuneState
{
	public record Invalid: RuneState
	{
		public override void SetRune(Rune rune)
		{
			rune.Visible = false;
		}
	}
	
	public record NotPlaced: RuneState
	{
		public override void SetRune(Rune rune)
		{
			rune.Visible = true;
			rune.Sprite.Modulate = new Color(1, 1, 1, 0.6f);
		}
	}

	public record Overlapping: RuneState
	{
		public override void SetRune(Rune rune)
		{
			rune.Visible = true;
			rune.Sprite.Modulate = new Color(1, 0, 0, 0.6f);
		}
	}
	
	public record Placed: RuneState
	{
		public override void SetRune(Rune rune)
		{
			rune.HighlightMesh.Visible = false;
			rune.Visible = true;
			rune.Sprite.Modulate = new Color(1, 1, 1);
		}
	}
	
	public record AimedAt: RuneState
	{
		public override void SetRune(Rune rune)
		{
			rune.HighlightMesh.Visible = true;
		}
	}

	public abstract void SetRune(Rune rune);

	public virtual bool Equals(RuneState? other)
	{
		return other is not null && GetType() == other.GetType();
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
