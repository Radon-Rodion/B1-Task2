import { getFiles } from "../requests/requests.js";
//import LoadFileForm from "../components/loadFileForm.jsx";
//import './pagesStyles.css';

class AllFilesPage extends React.Component {

    constructor(props) {
        super(props);
        this.state = { files: undefined };
        getFiles((data) => this.setState({ files: data }));
    }
    render() {
        if (!this.state.files)
            return (<div className="loading">Loading...</div>);
        return <>
            <div className="title">Loaded files:</div>
            <ul className="list">
                {
                    this.state.files.map((file) => {
                        return <li>
                            <a className="listElement" href={`api/files/${file.id}`} > {file.name} </a>
                        </li>;
                    }
                    )
                }
            </ul>
            <a className="btn btn-secondary" href="/api/files/load">Load file</a>
        </>;
    }
}

export default AllFilesPage;