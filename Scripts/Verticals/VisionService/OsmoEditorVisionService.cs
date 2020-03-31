using System.Collections.Generic;
using Byjus.Gamepod.RncStarRealms.Util;
using UnityEngine;
using Byjus.Gamepod.RncStarRealms.Externals;

#if !CC_STANDALONE
namespace Byjus.Gamepod.RncStarRealms.Verticals {

    /// <summary>
    /// Editor implementation of Vision Service
    /// If running on editor, this uses the tangible manager's dummy popup to bring deck objects on screen
    /// </summary>
    public class OsmoEditorVisionService : IVisionService {
        IOsmoEditorVisionHelper visionHelper;

        public OsmoEditorVisionService(IOsmoEditorVisionHelper visionHelper) {
            this.visionHelper = visionHelper;
        }

        public List<ExtInput> GetVisionObjects() {
            var aliveObjs = visionHelper.tangibleManager.AliveObjects;

            var ret = new List<ExtInput>();
            foreach (var obj in aliveObjs) {
                if (obj.Id < 10) {
                    ret.Add(new ExtInput { id = obj.Id, type = TileType.BLUE_ROD });
                } else {
                    ret.Add(new ExtInput { id = obj.Id, type = TileType.RED_CUBE });
                }
            }
            return ret;
        }

        Vector3 GetWorldPos(Vector3 editorPos) {
            var mDimens = CameraUtil.MainDimens();
            var edDimens = visionHelper.GetCameraDimens();
            var x = editorPos.x * (mDimens.x / edDimens.x);
            var y = editorPos.y * (mDimens.y / edDimens.y);

            return new Vector2(x, y);
        }

        public void Init() {

        }
    }
}
#endif