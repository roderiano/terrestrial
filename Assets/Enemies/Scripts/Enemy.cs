using UnityEngine;
using System.Collections;
 
public class Enemy : MonoBehaviour {
 
   private float health = 100;
   private EnemyStatus status = EnemyStatus.Idle;
   
   public void TakeDamage(float damage)
   {
      health -= damage;
   }
   
   public void SetStatus(EnemyStatus status) 
   {
      this.status = status;
   }

   public EnemyStatus GetStatus() 
   {
      return status;
   }
 
}