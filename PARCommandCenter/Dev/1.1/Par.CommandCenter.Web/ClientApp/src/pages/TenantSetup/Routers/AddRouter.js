import React from 'react';

import {
    Container,

} from "react-bootstrap";

const AddRouter = (props) => {

    return (
        <Container fluid>

            <div className="row">
                <div className="col-6">
                    <div id="selectRouterContent">
                        <div className="shadow border-0 card my-2 ">
                            <div className="card-body">
                                <h5 className="card-title text-center pr-4"><i className="fa-2x fas fa-cloud text-primary"></i></h5>
                                <p className="card-text text-center ">Configure Cloud Router Device (CDC).</p>
                                <div className="card-body text-center">
                                    <a onClick={props.onShowConfigureCloudRouterDevice} className="btn btn-primary ">Start Cloud Router</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="col-6">
                    <div className="shadow border-0 card my-2 ">
                        <div className="card-body">
                            <h5 className="card-title text-center pr-4"><i className="fa-2x fas fa-laptop-code text-primary"></i></h5>
                            <p className="card-text text-center ">Create Virtual Router Instance on Server</p>
                            <div className="card-body text-center">
                                <a onClick={props.onShowAddVMRouter} className="btn btn-primary ">Create VM Router</a>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="col-6">
                    <div className="shadow border-0 card my-2 ">
                        <div className="card-body">
                            <h5 className="card-title text-center pr-4"><i className="fa-2x fas fa-desktop text-primary"></i></h5>
                            {/*<p className="card-text text-center ">Cloud Router on a PC</p>*/}
                            <div className="card-body text-center">
                                <a onClick={props.onShowAddPcRouter} className="btn btn-primary ">Create Express PC Router</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </Container>
    );
}

export default AddRouter;