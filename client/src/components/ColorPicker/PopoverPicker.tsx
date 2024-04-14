import React, { useCallback, useEffect, useRef, useState } from "react";
import { RgbaColorPicker } from "react-colorful";

import classes from './style.module.scss'
import useClickOutside from "./useClickOutside";

export const PopoverPicker = ({ color, onChange }: any) => {
  const popover = useRef();
  const [isOpen, toggle] = useState(false);

  const close = useCallback(() => toggle(false), []);
  useClickOutside(popover, close);


  const componentToHex = (c : any) => {
    var hex = c.toString(16);
    return hex.length == 1 ? "0" + hex : hex;
  }

  const rgbToHex = (color: any) => {
    return "#" + componentToHex(color.r) + componentToHex(color.g) + componentToHex(color.b);
  }

  return (
    <>
      <div
        className={classes.swatch}
        style={{ backgroundColor: rgbToHex(color) }}
        onClick={() => toggle(true)}
      />
      <div className={classes.picker}>
        {isOpen && (
          //@ts-ignore
          <div className={classes.popover} ref={popover as React.RefObject<HTMLDivElement>}>
            <RgbaColorPicker color={color} onChange={onChange} />
          </div>
        )}
      </div>
    </>
  );
};