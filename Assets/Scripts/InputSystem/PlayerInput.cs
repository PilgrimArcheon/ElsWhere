using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
	[Header("Character Input Values")]
	public Vector2 move;
	public Vector2 look;
	public bool jump;
	public bool pause;
	public bool sprint;
	public bool aim;
	public bool pickUp;
	public bool useItemInputOrb, useItemInputHealth, useItemInputAmmo, useItemInputFood, useItemInputWater;
	public bool shoot;
	public bool reload;
	public bool toggleGun, toggleInventory;

	public bool isInteracting;

	[Header("Movement Settings")]
	public bool analogMovement;

	[Header("Mouse Cursor Settings")]
	public bool cursorLocked = true;
	public bool cursorInputForLook = true;

	public void OnMove(InputValue value)
	{
		MoveInput(value.Get<Vector2>());
	}

	public void OnLook(InputValue value)
	{
		if(cursorInputForLook)
		{
			LookInput(value.Get<Vector2>());
		}
	}

	public void OnJump(InputValue value)
	{
		JumpInput(value.isPressed);
	}

	public void OnPickUp(InputValue value)
	{
		PickInput(value.isPressed);
	}

	public void OnToggleGun(InputValue value)
	{
		GunInput(value.isPressed);
	}

	public void OnPause(InputValue value)
	{
		PauseInput(value.isPressed);
	}

	public void OnToggleInventory(InputValue value)
	{
		InventoryInput(value.isPressed);
	}

	public void OnUseItemInput_Orb(InputValue value)
	{
		UseItemInputOrb(value.isPressed);
	}
	public void OnUseItemInput_Health(InputValue value)
	{
		UseItemInputHealth(value.isPressed);
	}
	public void OnUseItemInput_Ammo(InputValue value)
	{
		UseItemInputAmmo(value.isPressed);
	}
	public void OnUseItemInput_Food(InputValue value)
	{
		UseItemInputFood(value.isPressed);
	}
	public void OnUseItemInput_Water(InputValue value)
	{
		UseItemInputWater(value.isPressed);
	}

	public void OnSprint(InputValue value)
	{
		SprintInput(value.isPressed);
	}

	public void OnAim(InputValue value)
	{
		AimInput(value.isPressed);
	}

	public void OnShoot(InputValue value)
	{
		ShootInput(value.isPressed);
	}

	public void OnReload(InputValue value)
	{
		ReloadInput(value.isPressed);
	}

	public void MoveInput(Vector2 newMoveDirection)
	{
		move = newMoveDirection;
	} 

	public void LookInput(Vector2 newLookDirection)
	{
		look = newLookDirection;
	}

	public void JumpInput(bool newJumpState)
	{
		jump = newJumpState;
	}

	public void PickInput(bool newPickUp)
	{
		pickUp = newPickUp;
	}

	public void GunInput(bool newGunState)
	{
		toggleGun = newGunState;
	}

	public void PauseInput(bool newPauseState)
	{
		pause = newPauseState;
	}

	public void InventoryInput(bool newInventoryState)
	{
		toggleInventory = newInventoryState;
	}

	public void UseItemInputOrb(bool newUseItemOrb)
	{
		useItemInputOrb = newUseItemOrb;
	}
	public void UseItemInputHealth(bool newUseItemHealth)
	{
		useItemInputHealth = newUseItemHealth;
	}
	public void UseItemInputAmmo(bool newUseItemAmmo)
	{
		useItemInputAmmo = newUseItemAmmo;
	}
	public void UseItemInputFood(bool newUseItemFood)
	{
		useItemInputFood = newUseItemFood;
	}
	public void UseItemInputWater(bool newUseItemWater)
	{
		useItemInputWater = newUseItemWater;
	}

	public void SprintInput(bool newSprintState)
	{
		sprint = newSprintState;
	}

	public void AimInput(bool newAimState)
	{
		aim = newAimState;
	}

	public void ShootInput(bool newShootState)
	{
		shoot = newShootState;
	}

	public void ReloadInput(bool newReloadState)
	{
		reload = newReloadState;
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		SetCursorState(cursorLocked);
	}

	private void SetCursorState(bool newState)
	{
		Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
	}
}