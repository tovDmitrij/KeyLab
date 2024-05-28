import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import Typography from "@mui/material/Typography";
import { CardActionArea } from "@mui/material";
import IconPlus from "../../Icons/IconPlus/IconPlus";

import classes from "./PreviewCard.module.scss";
import { useNavigate } from "react-router-dom";
import { FC } from "react";
import Preview from "../../List/SwitchesList/Preview";

const PreviewCard: FC<any> = ({ keyBoardId, title }) => {
  const navigate = useNavigate();

  return (
    <Card className={classes.card} sx={{ borderRadius: 10 }}>
      {!keyBoardId ? (
        <>
          <CardActionArea onClick={() => navigate("/constructors")}>
            <CardContent className={classes.card_content}>
              <IconPlus sx={{ fontSize: 50 }} />
              <Typography sx={{ mt: 1, fontSize: 20, color: "#AEAEAE" }}>
                добавить
              </Typography>{" "}
            </CardContent>
          </CardActionArea>
        </>
      ) : (
        <>
          <CardActionArea onClick={() => navigate(`/keyboard/${keyBoardId}`)}>
            <CardContent className={classes.card_content}>
              <Preview
                id={keyBoardId}
                type="keyboard"
                width={155}
                height={100}
              />
              <Typography sx={{ mt: 1, fontSize: 20, color: "#AEAEAE" }}>
                {title}
              </Typography>
            </CardContent>
          </CardActionArea>
        </>
      )}
    </Card>
  );
};

export default PreviewCard;
