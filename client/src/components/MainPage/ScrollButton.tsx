import { useEffect, useState } from "react";
import { IconButton } from "@mui/material";
import ArrowUpwardIcon from "@mui/icons-material/ArrowUpward";

const ScrollButton = () => {

  const [showScrollButton, setShowScrollButton] = useState(false);

  const scrollUp = () => {
    window.scrollTo({
      top: 0,
      left: 0,
      behavior: "smooth",
    });
  };

  useEffect(() => {
    const handleScroll = () => {
      if (window.scrollY > 100) {
        setShowScrollButton(true);
      } else {
        setShowScrollButton(false);
      }
    };

    window.addEventListener("scroll", handleScroll);

    return () => {
      window.removeEventListener("scroll", handleScroll);
    };
  }, []);

  return (
    <>
      {showScrollButton && (
        <IconButton
          size="small"
          sx={{ position: "fixed", bottom: "60px", right: "60px", color:"black", backgroundColor: "white" }}
          onClick={() => {
            scrollUp();
          }}
        >
          <ArrowUpwardIcon />
        </IconButton>
      )}
    </>
  );
};

export default ScrollButton;
