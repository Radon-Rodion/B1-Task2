import AllFilesPage from "./pages/allFilesPage.jsx";

class Hello extends React.Component {
    render() {
        return <AllFilesPage />;
    }
}
ReactDOM.render(
    <Hello />,
    document.getElementById("content")
);