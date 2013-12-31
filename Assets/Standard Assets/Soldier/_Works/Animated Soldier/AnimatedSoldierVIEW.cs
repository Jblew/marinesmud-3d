using UnityEngine;
using System.Collections;

public class AnimatedSoldierVIEW : MonoBehaviour {
	

	[System.Serializable]
	public class  Anim {
		public AnimationClip walk;
		public AnimationClip stand_Lshot;
		public AnimationClip stand_Rshot;
		public AnimationClip seat_Lshot;
		public AnimationClip seat_Fshot;
		public AnimationClip stand_Fshot;
		public AnimationClip idle;
		public AnimationClip hit;
		public AnimationClip death;
		public AnimationClip run;
	}
	
	
	public Anim anim;
	public Animation  aniBody;
	

	
	
	
	void Start () {
		for(int i = 0;i<4; i++){
			aniBody.clip = anim.idle;
			aniBody.Play ();
		}
	}
		

	
	void Update () {
		
			for(int i = 0;i<9; i++){
			if(Input.GetKey(KeyCode.Alpha1)){			
				aniBody.CrossFade ( anim.walk.name, 0.1f );
			}else if(Input.GetKey(KeyCode.Alpha2)){			
				aniBody.CrossFade ( anim.stand_Lshot.name, 0.1f );
			}else if(Input.GetKey(KeyCode.Alpha3)){			
				aniBody.CrossFade ( anim.stand_Rshot.name, 0.1f );
			}else if(Input.GetKey(KeyCode.Alpha4)){			
				aniBody.CrossFade ( anim.seat_Lshot.name, 0.1f );
			}else if(Input.GetKey(KeyCode.Alpha5)){			
				aniBody.CrossFade ( anim.seat_Fshot.name, 0.1f );
			}else if(Input.GetKey(KeyCode.Alpha6)){			
				aniBody.CrossFade ( anim.stand_Fshot.name, 0.1f );
			}else if(Input.GetKey(KeyCode.Alpha7)){			
				aniBody.CrossFade ( anim.idle.name, 0.1f );
			}else if(Input.GetKey(KeyCode.Alpha8)){			
				aniBody.CrossFade ( anim.hit.name, 0.1f );
			}else if(Input.GetKey(KeyCode.Alpha9)){			
				aniBody.CrossFade ( anim.death.name, 0.1f );
			}else if(Input.GetKey(KeyCode.Alpha0)){			
				aniBody.CrossFade ( anim.run.name, 0.1f );
			}
		}
	}
}
