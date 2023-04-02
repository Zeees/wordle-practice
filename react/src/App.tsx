import './App.scss';
import AppGameBoard from './components/AppGameBoard/AppGameBoard';

function App() {
  return (
    <>
      <header>
        <h1>Wordle - React Version</h1>
        <img src='logo192.png'></img>
      </header>
      <AppGameBoard></AppGameBoard>
    </>
  );
}

function logClick(): void {
  console.log("Button clicked")
}

export default App;
