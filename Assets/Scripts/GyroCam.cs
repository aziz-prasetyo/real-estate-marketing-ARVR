// best so far?
// https://developpaper.com/unity3d-uses-gyroscopes-to-control-node-rotation/
 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
using UnityEngine;
 
namespace Game.Gyro
{
 
 /// <summary>
 ///Responsibilities: 
 ///1. Realize the influence and operation of gyroscope on camera;
 ///2. Try to reproduce the main interface cockpit effect of crash 3;
 /// </summary>
 class GyroCam : MonoBehaviour
 {


  ///< summary > input type of gyroscope
  public enum EGyroInputType
  {
   /// <summary> RotateRate </summary>
   RotateRate,
 
   /// <summary> RotateRateUniased </summary>
   RotateRateUniased,
 
   /// <summary> UserAcceleration </summary>
   UserAcceleration,
  }
 
 
  public float m_gyro_max_x = 15.0f;
 
  public float m_gyro_max_y = 15.0f;
 
  public float m_gyro_max_z = 15.0f;
 
  ///Analog gyroscope input in < summary > editor development environment
  public Vector3 m_editor_debug_input = Vector3.zero;
 
  ///The input parameters of the < summary > gyroscope are used to control the camera
  public Vector3 m_gyro_input = Vector3.zero;
 
  ///< summary > current camera angle < / summary >
  public Vector3 m_cur_euler = Vector3.zero;
 
  ///< summary > update frequency of gyroscope data
  public int m_upate_rate = 30;
 
  ///< summary > input type of current gyroscope
  public EGyroInputType m_gyro_input_type = EGyroInputType.RotateRate;
 
  ///< summary > coefficient of gyroscope
  public float m_gyro_factor = 1.0f;
 
  private Vector3 m_camera_init_euler = Vector3.zero;
  private Transform mTransform;
 
  ///The input parameters of the < summary > gyroscope are used to control the camera
  protected Vector3 GyroInput
  {
   get
   {
    return m_gyro_input;
   }
   set
   {
    m_gyro_input = value;
   }
  }
 
  ///< summary > input data type of gyroscope
  protected EGyroInputType GyroInputType
  {
   get
   {
    return m_gyro_input_type;
   }
   set
   {
    m_gyro_input_type = value;
   }
  }
 
  ///< summary > coefficient of gyroscope
  protected float GyroFactor
  {
   get
   {
    return m_gyro_factor;
   }
   set
   {
    m_gyro_factor = value;
   }
  }
 
  ///< summary > current rotation angle
  protected Vector3 CurEuler
  {
   get
   {
    return m_cur_euler;
   }
   set
   {
    m_cur_euler = value;
   }
  }
 
  // Use this for initialization
  void Start()
  {
   Input.gyro.enabled = true;
 
   mTransform = gameObject.transform;
   CurEuler = mTransform.localEulerAngles;
   m_camera_init_euler = CurEuler;
  }
 
  ///< summary > Draw UI for debugging
  void OnGUI()
  {
   //GUI.Label(GetRect(0.1f, 0.05f), "Attitude: " + Input.gyro.attitude);
 
   //GUI.Label(GetRect(0.1f, 0.15f), "Rotation: " + Input.gyro.rotationRate);
 
   //GUI.Label(GetRect(0.1f, 0.25f), "RotationUnbiased: " + Input.gyro.rotationRateUnbiased);
 
   //GUI.Label(GetRect(0.1f, 0.35f), "UserAcceleration: " + Input.gyro.userAcceleration);
 
   ////Coefficient of gyroscope
   //{
   // string t_factor_str = GUI.TextField(GetRect(0.7f, 0.05f), "" + GyroFactor);
 
   // GyroFactor = float.Parse(t_factor_str);
   //}
 
   ////Input parameters of gyroscope
   //{
   // if (GUI.Button(GetRect(0.8f, 0.8f, 0.2f), "" + GyroInputType))
   // {
   //  switch (GyroInputType)
   //  {
   //   case EGyroInputType.RotateRate:
   //    GyroInputType = EGyroInputType.RotateRateUniased;
   //    break;
 
   //   case EGyroInputType.RotateRateUniased:
   //    GyroInputType = EGyroInputType.UserAcceleration;
   //    break;
 
   //   case EGyroInputType.UserAcceleration:
   //    GyroInputType = EGyroInputType.RotateRate;
   //    break;
   //  }
   // }
   //}
  }
 
  // Update is called once per frame
  void Update()
  {
   //Set gyro update frequency
   Input.gyro.updateInterval = 1.0f / m_upate_rate;
 
   //Calculate the control data of the camera according to the gyroscope
   UpdateGyro();
 

   //The gyroscope can not be used in the development environment to simulate the data
   GyroInput = m_editor_debug_input;

 
   //Because of the uncertain range of the value, the control coefficient should be added
   GyroInput = GyroInput * GyroFactor;
 
   //Operate and change the camera according to the control data
   UpdateCamera();
  }

 
  ///Update the gyroscope data and calculate the corresponding control data
  protected void UpdateGyro()
  {
   //Update the gyroscope data and calculate the control variables
   switch (GyroInputType)
   {// on the mobile phone, the left tilt x is negative, and the left tilt x is positive. Up tilt y is negative and down tilt y is positive
    case EGyroInputType.RotateRate:
     GyroInput = Input.gyro.rotationRate;
     break;
 
    case EGyroInputType.RotateRateUniased:
     GyroInput = Input.gyro.rotationRateUnbiased;
     break;
 
    case EGyroInputType.UserAcceleration:
     GyroInput = Input.gyro.userAcceleration;
     break;
 
    default:
     Debug.LogError("GyroInputTypeNot defined: " + GyroInputType);
     break;
   }
  }
 
  ///< summary > behavior of updating camera
  protected void UpdateCamera()
  {
   //The Z parameter of gyro is not required

   Vector3 t_gyro_input = new Vector3(GyroInput.x, GyroInput.y, GyroInput.z);

//    Vector3 t_gyro_input = new Vector3(0.0f, GyroInput.y, GyroInput.x);

 
   CurEuler += t_gyro_input;
 
   //Scope control
   {
    float t_x = ClampFloat(CurEuler.x, m_camera_init_euler.x, m_gyro_max_x);
 
    float t_y = ClampFloat(CurEuler.y, m_camera_init_euler.y, m_gyro_max_y);
 
    float t_z = ClampFloat(CurEuler.z, m_camera_init_euler.z, m_gyro_max_z);
 
    CurEuler = new Vector3(t_x, t_y, t_z);
   }
 
   mTransform.localEulerAngles = CurEuler;
  }
 
 
  protected float ClampFloat(float p_float, float p_init, float p_offset)
  {
   p_offset = Mathf.Abs(p_offset);
 
   if (p_float > p_init + p_offset)
   {
    p_float = p_init + p_offset;
   }
 
   if (p_float < p_init - p_offset)
   {
    p_float = p_init - p_offset;
   }
 
   return p_float;
  }
 
  ///< summary > get the approximate coordinates of GUI according to the percentage
  protected Rect GetRect(float p_x_percent, float p_y_percent, float p_w = 0.5f, float p_h = 0.1f)
  {
   return new Rect(
    Screen.width * p_x_percent, Screen.height * p_y_percent,
    Screen.width * p_w, Screen.height * p_h);
  }
 
 }
 
}