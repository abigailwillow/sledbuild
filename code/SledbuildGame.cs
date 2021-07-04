using Sandbox;

namespace Sledbuild {
	public partial class SledBuildGame : Sandbox.Game {
		public SledBuildGame() {
			if (IsServer) {
				// new SledbuildUI();
			}
		}
	}
}
