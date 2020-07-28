using UnityEngine;
using System.Collections;
 
public class Enemy : MonoBehaviour {
 
 private float health = 100;
 
 public void TakeDamage(float damage)
 {
    health -= damage;
 }
 
}