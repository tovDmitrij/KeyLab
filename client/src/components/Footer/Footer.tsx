import { Container } from "@mui/material";
import Typography from "@mui/material/Typography";

const Footer = () => {
    return (
    <footer
      id = "footer"
      // maxWidth={false}
      // disableGutters
    >
      <Typography fontSize={16} sx={{
            height: "50px",
            display: "flex",
            alignItems: "center",
            justifyContent: "center",
            color: "#AEAEAE"
            }}>
        2023-2024, Keylab
      </Typography>
    </footer>
  )
}

export default Footer;
