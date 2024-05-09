import React from "react";
import SvgIcon, { SvgIconProps } from "@mui/material/SvgIcon";

const IconPlus = (props: SvgIconProps) => {
  return (
    <SvgIcon {...props}>
      <svg width="61" height="61" viewBox="0 0 61 61" fill="none" xmlns="http://www.w3.org/2000/svg">
        <rect x="24.3999" width="13.0133" height="61" fill="#AEAEAE"/>
        <rect x="61" y="25.2134" width="13.0133" height="61" transform="rotate(90 61 25.2134)" fill="#AEAEAE"/>
      </svg>
    </SvgIcon>
  );
}

export default IconPlus;
