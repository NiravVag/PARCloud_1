import axios from "axios";
import React, { useEffect, useState } from 'react';

import {
    Alert,
    Badge,
    /*Button,*/
    ButtonGroup,
    Card,
    Form,
    InputGroup,
    Navbar,
    Nav,
    Table,
    Container,
    Row,
    Col,
    OverlayTrigger,
    Tooltip,
    ListGroup,
    Modal,
    FormControl,
    Spinner
} from "react-bootstrap";

import RouterConttolersList from "./RouterControllersList";

import { useDownloadFile } from "../../../../components/Common/CustomHooks/useDownloadFile";

import { Button } from "../../../../components/Common/Button/Button";

const PcRouterConfiguration = (props) => {

    const [isLoading, setIsLoading] = useState(false);
    const [selectedControllers, setSelectedControllers] = useState([]);

    const [downloadButtonState, setDownloadButtonState] = useState({ isLoading: false });

    const [showAlert, setShowAlert] = useState(false);

    const [alertMessage, setAlertMessage] = useState("");

    const preDownloading = () => setDownloadButtonState({ isLoading: true });
    const postDownloading = () => setDownloadButtonState({ isLoading: false });

    

    const onErrorDownloadFile = () => {
        setDownloadButtonState({ isLoading: false });
        setAlertMessage("Something went wrong. Please try again!")
        setShowAlert(true);
        setTimeout(() => {
            setShowAlert(false);
        }, 3000);
    };

    const getFileName = () => {        
        return `${(new Date().toJSON().slice(0, 10))}_Router_COWserver_PSSTART1.zip`;
    };

    const handleControllersSelection = (controllers) => {
        setSelectedControllers(controllers);
    }    

    const downloadConfigZipFile = () => {
       
            let controllers = selectedControllers.map(ctrl => `ControllerIds=${ctrl.id}`);
            let downloadUrl = `api/Router/DownloadPcRouterConfigZipFile?${controllers.join("&")}`;

            return axios.get(
                downloadUrl,
                {
                    responseType: "blob",
                    /* 
                    headers: {
                      Authorization: "Bearer <token>", // add authentication information as required by the backend APIs.
                    },
                     */
                }
            );
        

    };

    const { ref, url, download, name } = useDownloadFile({
        apiDefinition: downloadConfigZipFile,
        preDownloading,
        postDownloading,
        onError: onErrorDownloadFile,
        getFileName,
    });    


    const handleDownloadClick = () => {
        if (selectedControllers && selectedControllers.length > 0) {
            download();
        }
        else {
            setAlertMessage("You must select at least one controller");
            setShowAlert(true);
            setTimeout(() => {
                setShowAlert(false);
            }, 3000);
        }
    }

    return (
        <Container fluid>
            <Alert variant="danger" show={showAlert}>
                {alertMessage}
            </Alert>
            <Row>
                <Col md="12">
                    <RouterConttolersList routerId={props.routerId} onSelectionChanges={handleControllersSelection} />
                </Col>
            </Row>
            <Row className="d-flex justify-content-between">
                <Col md="4">

                </Col>
                <Col md="5" className="d-flex justify-content-end">
                    <a href={url} download={name} className="hidden" ref={ref} />                    
                    <Button label="Download" isLoading={downloadButtonState.isLoading} onClick={handleDownloadClick} />
                </Col>
            </Row>
        </Container>
    );
}


export default PcRouterConfiguration;