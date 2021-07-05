namespace Sandbox.Tools
{
	[Library( "tool_rope", Title = "Rope", Description = "Rope stuff together", Group = "construction" )]
	public partial class RopeTool : BaseTool
	{
		private Prop target;

		public override void Simulate()
		{
			if ( !Host.IsServer )
				return;

			using ( Prediction.Off() )
			{
				var startPos = Owner.EyePos;
				var dir = Owner.EyeRot.Forward;

				var tr = Trace.Ray( startPos, startPos + dir * MaxTraceDistance )
					.Ignore( Owner )
					.Run();

				if ( !tr.Hit || !tr.Body.IsValid() || !tr.Entity.IsValid() || tr.Entity.IsWorld )
					return;

				if ( tr.Entity.PhysicsGroup == null || tr.Entity.PhysicsGroup.BodyCount > 1 )
					return;

				if ( tr.Entity is not Prop prop )
					return;

				if ( Input.Pressed( InputButton.Attack1 ) )
				{
					if ( prop.Root is not Prop rootProp )
					{
						return;
					}

					if ( target == rootProp )
						return;

					if ( !target.IsValid() )
					{
						target = rootProp;
					}
					else
					{
						var rope = Particles.Create( "particles/rope.vpcf" );
						rope.SetEntity( 0, target );

						var attachEnt = tr.Body.IsValid() ? tr.Body.Entity : tr.Entity;
						var attachLocalPos = tr.Body.Transform.PointToLocal( tr.EndPos );

						if ( attachEnt.IsWorld )
						{
							rope.SetPosition( 1, attachLocalPos );
						}
						else
						{
							rope.SetEntityBone( 1, attachEnt, tr.Bone, new Transform( attachLocalPos ) );
						}

						PhysicsJoint.Spring
							.From( target.PhysicsBody )
							.To( tr.Body )
							.WithPivot( tr.EndPos )
							.WithFrequency( 5.0f )
							.WithDampingRatio( 0.7f )
							.WithReferenceMass( 0 )
							.WithMinRestLength( 0 )
							.WithMaxRestLength( 100 )
							.WithCollisionsEnabled()
							.Create();

						target = null;
					}
				}
				else
				{
					return;
				}

				CreateHitEffects( tr.EndPos );
			}
		}

		private void Reset()
		{
			target = null;
		}

		public override void Activate()
		{
			base.Activate();

			Reset();
		}

		public override void Deactivate()
		{
			base.Deactivate();

			Reset();
		}
	}
}
