
import Header from "../../components/Header/Header";
import AtomStats from "../../components/Stats/AtomStats";
import Graphs from "../../components/Stats/Graphs";
import Settings from "../../components/Stats/Settings/Settings";


const Stats = () => {

  return (
    <>
      <Header/>
      <Settings />
      <AtomStats />
      <Graphs /> 
    </>
  );
};

export default Stats;
