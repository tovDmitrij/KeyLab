import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import Typography from "@mui/material/Typography";
import { CardActionArea } from "@mui/material";
import IconPlus from "../../Icons/IconPlus/IconPlus";

import classes from "./PreviewCard.module.scss";
import { useNavigate } from "react-router-dom";

const PreviewCard = () => {
  const navigate = useNavigate();

  return (
    <Card className={classes.card} sx={{ borderRadius: 10 }}>
      <CardActionArea onClick={() => navigate("/constructors")}>
        <CardContent className={classes.card_content}>
          <IconPlus sx={{ fontSize: 50 }} />
          <Typography sx={{ mt: 1, fontSize: 20, color: "#AEAEAE" }}>
            добавить
          </Typography>
        </CardContent>
      </CardActionArea>
    </Card>
  );
};

export default PreviewCard;
