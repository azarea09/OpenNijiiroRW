namespace OpenNijiiroRW;
internal class CModalManager {

	public CLuaModalScript? lcModal { get; private set; }
	public ModalQueue rModalQueue {
		get;
		private set;
	}
	private Modal? displayedModals;

	public void RefleshSkin() {
		lcModal?.Dispose();
		lcModal = new CLuaModalScript(CSkin.Path("Modules/Modal"));

	}

	public void RegisterNewModal(int player, int rarity, Modal.EModalType modalType, params object?[] args) {
		lcModal?.RegisterNewModal(player, rarity, modalType, args);
	}

	public void Draw() {
		if (displayedModals != null) {
			lcModal?.Update();
			lcModal?.Draw();
		}
	}

	public bool Input() {
		if (OpenNijiiroRW.Pad.bPressedDGB(EPad.Decide)
			|| OpenNijiiroRW.InputManager.Keyboard.KeyPressed((int)SlimDXKeys.Key.Return)) {
			return InputManagement();
		}
		return false;
	}

	public bool InputManagement() {
		if ((OpenNijiiroRW.ModalManager.lcModal?.AnimationFinished() ?? true)) {
			OpenNijiiroRW.Skin.soundDecideSFX.tPlay(); // Include in module?

			if (!rModalQueue.tAreBothQueuesEmpty()
				&& (OpenNijiiroRW.Pad.bPressedDGB(EPad.Decide)
					|| OpenNijiiroRW.InputManager.Keyboard.KeyPressed((int)SlimDXKeys.Key.Return))) {
				displayedModals = rModalQueue.tPopModalInOrder();


			} else if (OpenNijiiroRW.ConfigIni.nPlayerCount == 1 || rModalQueue.tAreBothQueuesEmpty()) {

				if (!rModalQueue.tAreBothQueuesEmpty())
					LogNotification.PopError("Unexpected Error: Exited results screen with remaining modals, this is likely due to a Lua script issue.");

				displayedModals = null;
				return true;
			}
		}
		return false;
	}

	public CModalManager() {
		rModalQueue = new ModalQueue();
		displayedModals = null;
		lcModal = null;
	}



}
