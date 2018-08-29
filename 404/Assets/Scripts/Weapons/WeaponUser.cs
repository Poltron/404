using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure
{
	[RequireComponent(typeof(InteractiveBehaviour))]
	public class WeaponUser : MonoBehaviour
	{
		[SerializeField]
		private Weapon weapon;
		[SerializeField]
		private Transform weaponPosition;
		private InteractiveBehaviour interactiveBehaviour;
		private WeaponManager manager;

		private void Start()
		{
			manager = FindObjectOfType<WeaponManager>();
			interactiveBehaviour = gameObject.GetComponent<InteractiveBehaviour>();
			interactiveBehaviour.GetVariable("WEAPON").AddOnSetValue(Equip);
			Equip(interactiveBehaviour.GetVariable("WEAPON").Value);
		}

		private void OnDestroy()
		{
			interactiveBehaviour.GetVariable("WEAPON").RemoveOnSetValue(Equip);
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.LeftControl))
			{
				if (weapon == null)
					return;
				weapon.Use();
			}
		}

		private void ActiveWeapon()
		{
			if (weapon)
				weapon.ActiveWeapon();
		}

		private void Equip(string weaponValue)
		{
			if (weapon && weapon.Name == weaponValue)
				return;

			Weapon w = manager.GetWeapon(weaponValue);
			if (w == null)
				return;

			if (weapon)
				manager.DisableWeapon(weapon);

			weapon = w;
			weapon.transform.SetParent(weaponPosition);
			weapon.transform.position = weaponPosition.position;
			weapon.transform.localScale = Vector3.one;
		}
	}
}
