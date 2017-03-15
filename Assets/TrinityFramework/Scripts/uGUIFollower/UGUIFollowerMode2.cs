using UnityEngine;

/* UGUIFollowerMode2
  3D空間のオブジェクトを追いかけるUIコンポーネント
  CanvasRendererがカメラの時に適している
*/
public class UGUIFollowerMode2 : UGUIFollowerMode2Base
{
    #region Declaration
    // 使用中のカメラ 
    protected override Camera currentCamera
    {
        get { return Common.Camera.GetCamera(Define.Name.Camera.Main); }
    }
    #endregion
}