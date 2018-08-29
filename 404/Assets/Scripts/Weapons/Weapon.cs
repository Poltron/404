using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure
{
	public class Weapon : MonoBehaviour
	{
		[SerializeField]
		private WeaponData data;

		public string Name => data.VisualName;

		public virtual void Use()
		{
			if (data.Sound)
				FindObjectOfType<AudioManager>().PlaySound(data.Sound);
		}

		public virtual void ActiveWeapon()
		{
			Collider2D[] contact = Physics2D.OverlapBoxAll(transform.position, Vector2.one * data.Range, 0.0f);

			foreach (Collider2D col in contact)
			{
				EntityLife entity = col.gameObject.GetComponent<EntityLife>();
				if (entity == null || col.gameObject == gameObject)
					continue;
				entity.TakeDamage(data.Damage);
				Debug.Log("Touch " + entity.name);
			}
			Debug.Log("End Attack");
		}

		public virtual void Throw()
		{
			Debug.Log("throw " + data.VisualName);
		}
	}
}
