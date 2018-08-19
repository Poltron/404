using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure
{
	public class WeaponManager : MonoBehaviour
	{
		[SerializeField]
		private List<Weapon> allWeapons;
		private List<Weapon> instantiateWeapon;

		private void Awake()
		{
			instantiateWeapon = new List<Weapon>();
		}

		public Weapon GetWeapon(string weaponName)
		{
			Weapon weapon = instantiateWeapon.Find(w => w.Name == weaponName && w.gameObject.activeSelf == false);

			if (weapon)
			{
				weapon.gameObject.SetActive(true);
				return weapon;
			}

			weapon = allWeapons.Find(w => w.Name == weaponName);
			if (weapon)
			{
				weapon = Instantiate(weapon);
				instantiateWeapon.Add(weapon);
			}

			return weapon;
		}

		public void DisableWeapon(Weapon weapon)
		{
			weapon.transform.SetParent(transform);
			weapon.gameObject.SetActive(false);
		}
	}
}
