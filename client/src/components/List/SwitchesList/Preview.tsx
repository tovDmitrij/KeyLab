import { FC, useEffect, useRef, useState } from "react";
import { useGetSwitchPreviewQuery, useLazyGetSwitchPreviewQuery } from "../../../services/switchesService";
import SwitchPng from "./switch.png"; 

type props = {
  type: string;
  id?: string;
};

const Preview: FC<props> = ({ type, id }) => {
  const [previewImage, setPreviewImage] = useState<string | undefined>(undefined);
  const [switchPreview] = useLazyGetSwitchPreviewQuery();

  useEffect(() => {
    if (!id) return;
    if (type === "switch") {
      switchPreview(id)
      .unwrap()
      .then((data) => setPreviewImage(data));
    }
  }, [])

  return (
    {previewImage} && <img src={previewImage || SwitchPng}  width="65" height="65"/>
  );
};

export default Preview;
