using UnityEngine;

public class SpinningLoadingBar_2 : MonoBehaviour 
{
	private Vector3[] rotations = new Vector3[]
	{
		new Vector3( 0f, 0f, 0f ),
		new Vector3( 0f, 0f, 30f ),
		new Vector3( 0f, 0f, 60f ),
		new Vector3( 0f, 0f, 90f ),
		new Vector3( 0f, 0f, 120f ),
		new Vector3( 0f, 0f, 150f ),
		new Vector3( 0f, 0f, 180f ),
		new Vector3( 0f, 0f, 210f ),
		new Vector3( 0f, 0f, 240f ),
		new Vector3( 0f, 0f, 270f ),
		new Vector3( 0f, 0f, 300f ),
		new Vector3( 0f, 0f, 330f )
	};

	private Transform cachedTransform;

	private int index = 0;

	[Range( 0.01f, 1f )]
	public float animationDelay = 0.1f;
	public bool clockwise = true;
	public bool jigglyMovement = false;

	private float nextFrameTime = 0f;

	void Awake()
	{
		if( jigglyMovement )
			cachedTransform = transform;
		else
			cachedTransform = transform.Find( "Overlay" );

		nextFrameTime = Time.realtimeSinceStartup + animationDelay;
	}

	void Update()
	{
		float time = Time.realtimeSinceStartup;
		while( time >= nextFrameTime )
		{
			nextFrameTime += animationDelay;

			if( !clockwise )
				index = ( index + 1 ) % rotations.Length;
			else if( --index < 0 )
				index = rotations.Length - 1;

			cachedTransform.localEulerAngles = rotations[index];
		}
	}
}
