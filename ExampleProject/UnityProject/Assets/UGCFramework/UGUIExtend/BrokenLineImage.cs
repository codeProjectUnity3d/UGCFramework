﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

namespace UGCF.UGUIExtend
{
    public class BrokenLineImage : Image
    {
        public float height, width;
        public List<Vector2> allPoints = new List<Vector2>();

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            if (allPoints.Count == 0)
                base.OnPopulateMesh(vh);
            else
            {
                vh.Clear();

                float tw = rectTransform.rect.width;
                float th = rectTransform.rect.height;
                Vector2 pivotVector = new Vector2(tw * (0.5f - rectTransform.pivot.x), th * (0.5f - rectTransform.pivot.y));
                Vector4 uv = overrideSprite != null ? DataUtility.GetOuterUV(overrideSprite) : Vector4.zero;
                Vector2 center = new Vector2(uv.x + uv.z, uv.y + uv.w) * 0.5f - Vector2.one * 0.5f;
                float uvScaleX = (uv.z - uv.x) / tw;
                float uvScaleY = (uv.w - uv.y) / th;
                Vector2 halfSize = rectTransform.rect.size / 2;

                UIVertex uiVertex;
                int verticeCount;
                int triangleCount;
                Vector2 curVertice;
                verticeCount = allPoints.Count;

                for (int i = 0; i < verticeCount; i++)
                {
                    curVertice = new Vector2(allPoints[i].x * tw / width, allPoints[i].y * th / height);
                    uiVertex = new UIVertex
                    {
                        color = color,
                        position = curVertice - halfSize + pivotVector,
                        uv0 = new Vector2(curVertice.x * uvScaleX, curVertice.y * uvScaleY) + center
                    };
                    vh.AddVert(uiVertex);
                }
                for (int i = allPoints.Count - 3; i > 1; i--)
                {
                    verticeCount++;
                    curVertice = Vector2.right * allPoints[i].x * tw / width;
                    uiVertex = new UIVertex
                    {
                        color = color,
                        position = curVertice - halfSize + pivotVector,
                        uv0 = new Vector2(curVertice.x * uvScaleX, curVertice.y * uvScaleY) + center
                    };
                    vh.AddVert(uiVertex);
                }
                triangleCount = verticeCount - 2;
                for (int i = 0, vIdx = triangleCount / 2 + 1; i < triangleCount / 2; i++, vIdx--)
                {
                    if (vIdx + i * 2 + 2 <= verticeCount - 1)
                    {
                        vh.AddTriangle(vIdx, vIdx + i * 2 + 1, vIdx + i * 2 + 2);
                        vh.AddTriangle(vIdx, vIdx + i * 2 + 2, vIdx - 1);
                    }
                    else
                    {
                        vh.AddTriangle(vIdx, vIdx + i * 2 + 1, 0);
                        vh.AddTriangle(vIdx, 0, vIdx - 1);
                    }
                }
            }
        }
    }
}