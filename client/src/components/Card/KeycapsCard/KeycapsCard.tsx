import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import Typography from "@mui/material/Typography";
import { CardActionArea } from "@mui/material";
import IconPlus from "../../Icons/IconPlus/IconPlus";

import classes from "./KeycapsCard.module.scss";
import { useNavigate } from "react-router-dom";
import { useAppSelector } from "../../../store/redux";
import Preview from "../../List/SwitchesList/Preview";

const KeycapsCard = () => {
  const navigate = useNavigate();
  const { kitID, kitTitle } = useAppSelector(
    (state) => state.keyboardReduer
  );

  return (
    <Card className={classes.card} sx={{ borderRadius: 10 }}>
      <CardActionArea onClick={() => navigate("/constrKeys")}>
        <CardContent className={classes.card_content}>
        {!kitTitle ? (
            <>
              <Typography sx={{ mt: 1, fontSize: 20, color: "#AEAEAE" }}>
                Выберите набор клавиш
              </Typography>
              <IconPlus sx={{ fontSize: 50 }} />
            </>
          ) : (
            <>
              <Preview id={kitID} type="kit" width={300} height={165} />
              <Typography sx={{ mt: 1, fontSize: 20, color: "#AEAEAE" }}>
                {kitTitle}
              </Typography>
            </>
          )}
        </CardContent>
      </CardActionArea>
    </Card>
  );
};

export default KeycapsCard;
