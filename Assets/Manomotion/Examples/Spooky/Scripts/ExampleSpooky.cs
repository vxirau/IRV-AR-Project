using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ExampleSpooky : MonoBehaviour
{
	[SerializeField]
	float fadeSpeed = 5;
    public Text triggertext;
    public float shotPower = 100f;

    Image spookeyImageHolder;
    public GameObject bow;
	RectTransform spookyRectTransform;
    public GameObject arrowPrefab;
    public GameObject RocketPrefab;
    public Transform arrowLocation;
    [SerializeField]
	Sprite openHandSprite;
	[SerializeField]
	Sprite closedHandSprite;
    public bool grabflag;
    public Button energybar;
    public Text arrowCountText;

	public int currentArrows;
	public int currentRockets;

	public int totalArrows;
	public int totalRockets;

	public int currentLevel;
	 public Text currentLevelText;

    public Text rocketCountText;
	public GameUIcontroller gameUIcontroller;

    public float f;
	// Use this for initialization
	void Start()
	{
        f = 800;
        grabflag = false;
		//spookeyImageHolder = this.GetComponent<Image>();
	//	spookeyImageHolder.preserveAspect = true;
	//	spookyRectTransform = this.GetComponent<RectTransform>();
		ManomotionManager.OnManoMotionFrameProcessed += HandleManoMotionFrameUpdated;
        if (arrowLocation == null)
            arrowLocation = transform;
    }


	/// <summary>
	/// Handles the information from the processed frame in order to use the gesture and tracking information to illustrate Spooky.
	/// </summary>
	void HandleManoMotionFrameUpdated()
	{
		GestureInfo gesture = ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info;
		TrackingInfo tracking = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info;
		Warning warning = ManomotionManager.Instance.Hand_infos[0].hand_info.warning;

		AssignSpookeyFace(gesture, warning);
		MoveAndScaleSpookey(tracking, warning);
		HighlightSpookeyImage(warning);
	}

	/// <summary>
	/// Places the visualization of the ghost into the center of the bounding box and updates its position based on the detection.
	///  The scale of the image will depend based on the width or height (depends which one is bigger) of the bounding box. The Image is set to preserve aspect in order to prevent any disform
	/// </summary>
	/// <param name="trackingInfo">Tracking info.</param>
	/// <param name="warning">Warning.</param>
	void MoveAndScaleSpookey(TrackingInfo trackingInfo, Warning warning)
	{
		if (warning != Warning.WARNING_HAND_NOT_FOUND)
		{
         //   spookyRectTransform.position = Camera.main.ViewportToScreenPoint(trackingInfo.palm_center);
         //   float width = Screen.width * trackingInfo.bounding_box.width;
         //  float height = Screen.height * trackingInfo.bounding_box.height;

          //  	float size = Mathf.Min(width, height);
        //    bow.transform.position = trackingInfo.palm_center;
     //   spookyRectTransform.sizeDelta = new Vector2(size, size);
		}
	}
    void EnergyBar()
    {
       
    }


	public void newLevel(){
		currentLevel = currentLevel+1;
		totalArrows = totalArrows+10;
		currentArrows = totalArrows;
		totalRockets = totalRockets+2;
		currentRockets = totalRockets;
		rocketCountText.text = currentRockets.ToString();
		arrowCountText.text = currentArrows.ToString();
		currentLevelText.text = "Level " + currentLevel.ToString();
	}

	/// <summary>
	/// Based on the continuous gesture performed (Open hand or Closed Hand) the ghost will change its appearance
	/// </summary>
	/// <param name="gesture">Gesture.</param>
	/// <param name="warning">Warning.</param>
	void AssignSpookeyFace(GestureInfo gesture, Warning warning)
	{
		if (warning != Warning.WARNING_HAND_NOT_FOUND)
		{
			if(currentRockets == 0 && currentArrows == 0){
				gameUIcontroller.GameOver();
			}else{
				switch (gesture.mano_gesture_trigger)
				{
					case ManoGestureTrigger.GRAB_GESTURE:
						if( currentRockets > 0){
							grabflag = true;
							triggertext.text = "CARGANDO COHETE...";
						}else{
							triggertext.text = "NO HAY COHETES!";
						}
						break;
					case ManoGestureTrigger.RELEASE_GESTURE:
						if( currentRockets > 0){
							shotPower = Mathf.Lerp(800, 2800, f);
							triggertext.text = "DISPARO!";
							Instantiate(RocketPrefab, arrowLocation.position, arrowLocation.rotation).GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * shotPower);
							grabflag = false;
							currentRockets--;
							rocketCountText.text = currentRockets.ToString();
						}else{
							triggertext.text = "NO HAY COHETES!";
						}
						break;
					case ManoGestureTrigger.CLICK:
						if(currentArrows >0){
							grabflag = false;
							shotPower = 2500;
							f = 0;
							Instantiate(arrowPrefab, arrowLocation.position, arrowLocation.rotation).GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * shotPower);
							triggertext.text = "FLECHA!";
							currentArrows--;
							arrowCountText.text = currentArrows.ToString();
						}else{
							triggertext.text = "NO HAY FLECHAS!";
						}
						break;
					default:
						break;
						

				}
			}
		}
	}

	/// <summary>
	/// If there is no hand detected Spooky will become transparent and fade out, while if the hand is being detected the image of the ghost will turn back on visible.
	/// </summary>
	/// <param name="warning">Warning.</param>
	void HighlightSpookeyImage(Warning warning)
	{
		if (warning == Warning.WARNING_HAND_NOT_FOUND)
		{
			FadeOut(spookeyImageHolder);
		}
		else
		{
			FadeIn(spookeyImageHolder);
		}
	}

	/// <summary>
	///Gradually decreases the alpha value of the image to create the effect of fading out
	/// </summary>
	/// <param name="image">Image.</param>
	void FadeOut(Image image)
	{
		Color currentColor = image.color;
		if (currentColor.a > 0)
		{
			currentColor.a -= Time.deltaTime * fadeSpeed;
		}
		image.color = currentColor;
	}

	/// <summary>
	/// Gradually increases the alpha value of the image to create the effect of fading in
	/// </summary>
	/// <param name="image">Image.</param>
	void FadeIn(Image image)
	{
		Color currentColor = image.color;
		if (currentColor.a < 1)
		{
			currentColor.a += Time.deltaTime * fadeSpeed;
		}
		image.color = currentColor;
	}
      void Update()
    {
        energybar.gameObject.SetActive(grabflag);

        if (grabflag)
        {
             f = Mathf.PingPong(1f * Time.time, 1);
            energybar.GetComponent<Image>().fillAmount = f;
            if(f<0.33f)
            {
                energybar.GetComponent<Image>().color = Color.green;
            }
                if(f>=0.33f && f<0.66f)
            {
                energybar.GetComponent<Image>().color = Color.yellow;

            }
            if (f>0.66f)
            {
                energybar.GetComponent<Image>().color = Color.red;

            }
        }

    }
}
