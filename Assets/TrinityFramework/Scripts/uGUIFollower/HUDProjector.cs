using System;
using UnityEngine;

public class HUDProjector : HUDProjectorBase
{

    #region Declaration
    /*
    TODO:一旦CameraManagerの再設計が終わるまでコメントアウト
    */
    // 3D空間を見ているカメラ
    //protected override Camera currentCamera
    //{
    //    get { return Common.Camera.CurrentCamera; }
    //}
    //// 常に手前に表示したいUI用等のカメラ
    //protected override Camera uiCamera
    //{
    //    get { return Common.Camera.UiCamera; }
    //}
    #endregion

    #region Public Method
    // 位置を追従させる
    public void UpdatePosition()
    {
        Vector3 position;
        // オブジェクトをフォローする場合はオブジェクトのポジションでUpdate
        if (isFollow)
        {
            position = targetObject.Transform.position;
        }
        // オブジェクトをフォローしない場合は指定の座標でUpdate
        else
        {
            position = _fixPosition;
        }
        // カメラに向きを合わせる
        Transform.rotation = uiCamera.transform.rotation;
        // 中央のポジションを取得
        var centerScreenPosition = currentCamera.WorldToScreenPoint(position + offset);
        // 範囲指定がある場合は範囲内に収まるようにClampする
        if (isMovingRange)
        {
            centerScreenPosition = ClampMovingRange(centerScreenPosition);
        }
        var worldPosition = uiCamera.ScreenToWorldPoint(new Vector3(centerScreenPosition.x + ScreenOffset.x, centerScreenPosition.y + ScreenOffset.y, cameraRange));
        Transform.position = worldPosition;
    }
    #endregion
}