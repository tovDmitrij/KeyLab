import { FC, useEffect, useState } from "react";
import { useLazyGetSwitchPreviewQuery } from "../../../services/switchesService";
import { useLazyGetBoxPreviewQuery } from "../../../services/boxesService";
import { useLazyGetKitPreviewQuery } from "../../../services/kitsService";
import { useLazyGetKeyBoardPreviewQuery } from "../../../services/keyboardService";

type Props = {
  id?: string;
  type?: string;
  width?: number;
  height?: number;
  scale?: number;
};

const Preview: FC<Props> = ({ width, height, type, id, scale }) => {
  const [previewImage, setPreviewImage] = useState<string | undefined>(undefined);
  const [getSwitchPreview] = useLazyGetSwitchPreviewQuery();
  const [getBoxPreview] = useLazyGetBoxPreviewQuery();
  const [getKitPreview] = useLazyGetKitPreviewQuery();
  const [getKeyboardPreview] = useLazyGetKeyBoardPreviewQuery();

  useEffect(() => {
    if (!id || !type) return;

    const fetchPreview = async () => {
      let data;
      if (type === "switch") {
        data = await getSwitchPreview(id).unwrap();
      } else if (type === "box") {
        data = await getBoxPreview(id).unwrap();
      } else if (type === "kit") {
        data = await getKitPreview(id).unwrap();
      } else if (type === "keyboard") {
        data = await getKeyboardPreview(id).unwrap();
      }

      if (data) {
        setPreviewImage(data.previewBase64);
      }
    };

    fetchPreview();
  }, [id, type, getSwitchPreview, getBoxPreview, getKitPreview, getKeyboardPreview]);

  return (
    previewImage && (
      <img
        src={`data:image/webp;base64,${previewImage}`}
        style={{ transform: `scale(${scale || 1.1})` }}
        width={width}
        height={height}
        draggable="false"
      />
    )
  );
};

export default Preview;
