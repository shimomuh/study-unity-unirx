using UnityEngine;

namespace Ui
{
	public class ScaledRect
	{

		public static int DEFAULT_BASE_WIDTH = 640;
		public static int DEFAULT_BASE_HEIGHT = 960;

		float scaleAspect;
		Vector2 offset;

		public ScaledRect() : this(DEFAULT_BASE_WIDTH, DEFAULT_BASE_HEIGHT)
		{
		}

		public ScaledRect(int baseWidth, int baseHeight)
		{
			bool landscape = Screen.orientation == ScreenOrientation.Landscape;
			float width = landscape ? baseHeight : baseWidth;
			float height = landscape ? baseWidth : baseHeight;

			float baseAspect = width / height;
			float realAspect = (float) Screen.width / (float) Screen.height;
			float scale = realAspect / baseAspect;

			Rect offsetRct = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
			if (1.0f > scale)
			{
				offsetRct.x = 0;
				offsetRct.width = 1.0f;
				offsetRct.y = 1.0f - scale;
				offsetRct.height = scale;

				scaleAspect = (float) Screen.width / width;
			}
			else
			{
				scale = 1.0f / scale;
				offsetRct.x = 1.0f - scale;
				offsetRct.width = scale;
				offsetRct.y = 0.0f;
				offsetRct.height = 1.0f;

				scaleAspect = (float) Screen.height / height;
			}


			offset.x = offsetRct.x * Screen.width;
			offset.y = offsetRct.y * Screen.height;
		}


		public float CalcSize(float size)
		{
			return size * scaleAspect;
		}

		public Rect CalcRect(Rect rect)
		{
			return CalcRect(rect.x, rect.y, rect.width, rect.height);
		}

		public Rect CalcRect(float x, float y, float width, float height)
		{
			float offx = offset.x;
			float offy = offset.y;
			if ((x + width) / (float) Screen.width < 0.75f)
			{
				offx = offset.x * (x / Screen.width);
			}

			if ((y + height) / (float) Screen.height < 0.75f)
			{
				offy = offset.y * (y / Screen.height);
			}

			Rect rect = new Rect(
				offx + (x * scaleAspect),
				offy + (y * scaleAspect),
				width * scaleAspect,
				height * scaleAspect
			);
			return rect;
		}
	}
}