using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
	static ChangeScene instance;
	public string scene_name;
	// Start is called before the first frame update

	// Update is called once per frame
	static Scene m_Scene;
	void Awake()
	{
		m_Scene = SceneManager.GetActiveScene();
		scene_name = m_Scene.name;
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}
	private void Start()
	{

	}
	void Update()
	{
		scene_name = SceneManager.GetActiveScene().name;
	}
	
}
