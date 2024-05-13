import { FC, useEffect, useRef, useState } from "react";
import { useLazyGetSwitchPreviewQuery } from "../../../services/switchesService";
import { useLazyGetBoxPreviewQuery } from "../../../services/boxesService";
import { useLazyGetKitPreviewQuery } from "../../../services/kitsService";

type props = {
  id?: string;
  type?: string;
  width?: number;
  height?: number;
  scale?: number;
};

const Preview: FC<props> = ({ width, height, type, id, scale}) => {
  const [previewImage, setPreviewImage] = useState<string | undefined>(undefined);
  const [switchPreview] = useLazyGetSwitchPreviewQuery();
  const [boxPreview] = useLazyGetBoxPreviewQuery();
  const [kitPreview] = useLazyGetKitPreviewQuery();


  useEffect(() => {
    if (!id) return;
    if (type === "switch") {
      switchPreview(id)
      .unwrap()
      .then((data) => setPreviewImage(data.previewBase64));
    }
    if (type === "box") {
      boxPreview(id)
      .unwrap()
      .then((data) => setPreviewImage(data.previewBase64));
    }
    if (type === "kit") {
      kitPreview(id)
      .unwrap()
      .then((data) => {console.log(data), setPreviewImage(data.previewBase64)})
    }
  }, [])
  
  return (
    {previewImage} && <img src={`data:image/webp;base64,${previewImage}`} style={{
      transform: "scale(1.1, 1.1)",
      }} width={width} height={height} draggable="false"/>
  );
};

export default Preview;
