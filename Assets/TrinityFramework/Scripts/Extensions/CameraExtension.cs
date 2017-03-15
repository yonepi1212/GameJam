using UnityEngine;
/* CameraExtension
    カメラのレイヤーを指定しやすくするクラス
 */
public static class CameraExtension
{
    public static void AddLayer(this Camera camera, string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        camera.cullingMask |= (1 << layer);
    }

    public static void RemoveLayer(this Camera camera, string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        camera.cullingMask &= ~(1 << layer);
    }

    public static void InitializeLayer(this Camera camera, string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        camera.cullingMask = 1 << layer;
    }
}
