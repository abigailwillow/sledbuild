using Sandbox;

namespace SledBuild
{
	[Library( "func_sled_track", Description = "The track that sleds will sled on." )]
	[Hammer.Solid]
	public class FuncSledTrack : BaseTrigger
	{
		public override bool PassesTriggerFilters( Entity other )
		{
			if ( other is not ModelEntity modelEnt )
				return false;

			if ( other is Prop || modelEnt.CollisionGroup == CollisionGroup.Player) {
				return true;
			}

			return false;
		}

		public override void OnTouchStart( Entity other )
		{
			if ( other == null )
			{
				return;
			}

			base.OnTouchStart( other );

			if ( other is Prop prop )
			{
				if ( prop.PhysicsBody == null || prop.PhysicsBody.PhysicsGroup == null )
				{
					return;
				}

				prop.PhysicsBody.PhysicsGroup.SetSurface( "ice" );
			}
		}

		public override void OnTouchEnd( Entity other )
		{
			if ( other == null )
			{
				return;
			}

			base.OnTouchEnd( other );

			if ( other is Prop prop )
			{
				if ( prop.PhysicsBody == null || prop.PhysicsBody.PhysicsGroup == null )
				{
					return;
				}

				prop.PhysicsBody.PhysicsGroup.SetSurface( "default" );
			}
		}
	}
}
