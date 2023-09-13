import savedGif from '../../../styles/images/ParSaved.gif';
import ReactDOM from 'react-dom';
import React, { useEffect, useState } from 'react';

import {
    Badge,
    Button,
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

const ConfigureCloudRouterDevice = (props) => {
    useEffect(() => {
        fetch('api/Router/GetAllRoutersUnassigned')
            .then(response => response.json())
            .then(data => {
                //  let selectedRouters = [];

                if (data.routers.length > 0) {
                    setCloudRouterArray(data.routers);
                }
                // Add router to router array.


            });

    }, []);

    const [cloudRouterArray, setCloudRouterArray] = useState([]);

    const [cloudRouterAddress, setCloudRouterAddress] = useState("");

    const [showPortSetup, setShowPortSetup] = useState(false);


    const [showCDCSetup, setShowCDCSetup] = useState(true);

    const [checkedSmart1Batch, setCheckedSmart1Batch] = useState(false);

    const [selectedCloudRouter, setSelectedCloudRouter] = useState();

    const [step1LoaderCDC, setStep1LoaderCDC] = useState(false);

    const [foundRouterCDC, setFoundRouterCDC] = useState();
    const [showCDCInput, setShowCDCInput] = useState(true);
    const [errorCDC, setErrorCDC] = useState(false);
    const [successCDC, setSuccessCDC] = useState(false);

    const [showCloudRouterInfo, setShowCloudRouterInfo] = useState(false);

    
    let routerConfigCDC = <h1>TEST</h1>
 
    const checkNewCloudRouter = async (valPassed) => {

        let foundValue = false;
        setCloudRouterAddress(valPassed);
        if (valPassed.length == 14) {
            
            console.log(cloudRouterArray)
            for (var i = 0; i < cloudRouterArray.length; i++) {
                //FOUND ROUTER 

                if (cloudRouterArray[i].address == valPassed) {
                    foundValue = true;
                   
                 

                    let routerConfigCDC = <div className="card-data dataCardCR my-3 card">
                      
                        <ul className="listview flush transparent image-listview">
                            <li>
                                <div className="item">
                                    <div className="icon-box bg-white"><i className="text-primary fas fa-wifi"></i></div>


                                    <div className="in">{cloudRouterArray[i].address}<div onClick={() => { setShowCDCInput(true); setShowPortSetup(false) }} className="mt-1 btn btn-sm btn-remove-color ">Remove</div></div>
                                </div>
                            </li>
                            <li>
                                <div className="item">
                                    <div className="icon-box bg-white"><i className="text-primary fas fa-cloud-download-alt"></i></div>
                                    <div className="in">Firmware<span className="text-right pl-05 locationNameRO text-bold">{cloudRouterArray[i].firmwareVersion}</span></div>
                                </div>
                            </li>
                            <li>
                                <div className="item">
                                    <div className="icon-box bg-white"><i className="text-primary fas fa-signal"></i></div>
                                    <div className="in">Router Status<span className="text-right text-bold">{cloudRouterArray[i].isRunning ? <div className="text-success">Online</div> : <div className="text-danger">Offline</div> }</span></div>
                                </div>
                            </li>
                        </ul>
                    </div>











                     
                    setFoundRouterCDC(routerConfigCDC)


               
                  
                    setShowPortSetup(true)



                    let portsArray = [];

                    portsArray.push({ portType: 'smart1', parChargeBatch: false, parChargeMode: false })
                    portsArray.push({ portType: 'smart2', parChargeBatch: false, parChargeMode: false })
                    portsArray.push({ portType: 'classicA', parChargeBatch: false, parChargeMode: false })
                    portsArray.push({ portType: 'classicB', parChargeBatch: false, parChargeMode: false })
                    portsArray.push({ portType: 'classicC', parChargeBatch: false, parChargeMode: false })
                    portsArray.push({ portType: 'classicD', parChargeBatch: false, parChargeMode: false })
                    portsArray.push({ portType: 'classicE', parChargeBatch: false, parChargeMode: false })
                    portsArray.push({ portType: 'classicF', parChargeBatch: false, parChargeMode: false })


                    let objCloudRouter = {
                        routerAddress: valPassed, ports: portsArray, tenantId: props.tenantId
                    };

                    setSelectedCloudRouter(objCloudRouter);
                    setShowCDCInput(false)
                    console.log(objCloudRouter);
                    
                    break;
                }
            }
        }

        if (foundValue == false) {

            setShowPortSetup(false);

        }

    }





    if (foundRouterCDC)
    {
        routerConfigCDC = foundRouterCDC
    }


    const portsChecked = async (e) => {

        setCheckedSmart1Batch(e.target.value)
        let idOfInput = e.target.id;
        //objCloudRouter
        let nameOfInput = e.target.getAttribute("data-port");
        console.log(idOfInput);
        console.log(nameOfInput);


        let ports = selectedCloudRouter.ports;
        for (var i = 0; i < ports.length; i++) {
            if (idOfInput.includes(ports[i].portType)) {

                if (idOfInput.includes('RapidRead')) {
                    if (e.target.checked == true) {
                        ports[i].parChargeMode = true
                    }
                    else {
                        ports[i].parChargeMode = false
                    }

                }

                else if (idOfInput.includes('BatchMode')) {
                    if (e.target.checked == true) {
                        ports[i].parChargeBatch = true
                    }
                    else {
                        ports[i].parChargeBatch = false
                    }

                }

                break;
            }

            setSelectedCloudRouter({ routerAddress: cloudRouterAddress, ports: ports, tenantId: props.tenantId });
        }

        console.log(selectedCloudRouter)

    }
        
    const registerNewCloudRouter = async () => {

        // let portsArray  = 
        try {
            const url = "api/Router/CreateCloudRouter";
            setStep1LoaderCDC(true)
            setShowCDCSetup(false)

            /*setShowVMInfo(false)*/
            //setShowCloudText(false)
            setShowCloudRouterInfo(false)

            const response = await fetch(url,
                {
                    method: "post",
                    headers: new Headers({
                        "Content-Type": "application/json",
                        Accept: "application/json"
                    }),
                    body: JSON.stringify(selectedCloudRouter),
                });


            if (response.status === 400) {
                let responseBody = await response.json();

            }
            else if (response.status === 200) {
              
                let responseBody = await response.json();
                console.log(responseBody)
                let newRouter = {
                    routerId: responseBody.routerId,
                    routerAddress: responseBody.routerAddress,
                }
                console.log(responseBody)
                setSuccessCDC(true)
                setTimeout(() => {
                    closeModal()
                    
                 //   alert('WOULD CLOSE')
                    //  setStep1IsLoading(false);
                    //       setStep1Completed(true);

                    // Add new cloud router.
                    //     createNewCloudRouter(newRouter);
                }, 3000);

                //return newRouter;

            } else {
                setStep1LoaderCDC(false)

                setErrorCDC(true)
                //setStep1Error("An error occured during registration.");
            }


        } catch (e) {
            console.log(e);
            //setStep1Error("An error occured during registration.");
        }


        setStep1LoaderCDC(false)

    }

    const closeModal = () => {

   
    props.onAddRouterComplete();
}
    let loaderCDC = (step1LoaderCDC) ?
        <div className="row m-5">
            <div className="col m-5">
                <div className="wifi-loader">
                    <svg className="circle-outer" viewBox="0 0 86 86">
                        <circle className="back" cx="43" cy="43" r="40"></circle>
                        <circle className="front" cx="43" cy="43" r="40"></circle>
                        <circle className="new" cx="43" cy="43" r="40"></circle>
                    </svg>
                    <svg className="circle-middle" viewBox="0 0 60 60">
                        <circle className="back" cx="30" cy="30" r="27"></circle>
                        <circle className="front" cx="30" cy="30" r="27"></circle>
                    </svg>
                    <svg className="circle-inner" viewBox="0 0 34 34">
                        <circle className="back" cx="17" cy="17" r="14"></circle>
                        <circle className="front" cx="17" cy="17" r="14"></circle>
                    </svg>
                    <div className=" cloudRouterLoaderText text" data-text="Creating CDC Router"></div>
                </div>
            </div>
        </div>
        : undefined;





   
    let loaderCDCError = (errorCDC) ?

        <div className="row m-5">
            <div className="text-center col p-0 ">
                <div className="dangerIconGif">





                    <div className="ui-error">
                        <svg viewBox="0 0 87 87" version="1.1">
                            <g id="Page-1" stroke="none" strokeWidth="1" fill="none" fillRule="evenodd">
                                <g id="Group-2" transform="translate(2.000000, 2.000000)">
                                    <circle id="Oval-2" stroke="rgba(252, 191, 191, .5)" strokeWidth="4" cx="41.5" cy="41.5" r="41.5"></circle>
                                    <circle className="ui-error-circle" stroke="#F74444" strokeWidth="4" cx="41.5" cy="41.5" r="41.5"></circle>
                                    <path className="ui-error-line1" d="M22.244224,22 L60.4279902,60.1837662" id="Line" stroke="#F74444" strokeWidth="3" strokeLinecap="square"></path>
                                    <path className="ui-error-line2" d="M60.755776,21 L23.244224,59.8443492" id="Line" stroke="#F74444" strokeWidth="3" strokeLinecap="square"></path>
                                </g>
                            </g>
                        </svg>
                    </div>



                </div>
                <h5>Oops! There was an Error Creating the CDC Controller</h5>
            </div>
        </div>
        : undefined;



    let loaderCDCSuccess = (successCDC) ?
        <div className="row m-5">
            <div className="text-center col p-0 ">
                <div className="dangerIconGif">
                    <div className="ui-error">
                        <svg className="checkmark" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 52 52">
                            <circle className="checkmark__circle" cx="26" cy="26" r="25" fill="none" />
                            <path className="checkmark__check" fill="none" d="M14.1 27.2l7.1 7.2 16.7-16.8" />
                        </svg>
                    </div>
                </div>
                <h5>CDC Controller Successfully Created</h5>
            </div>
        </div>
        : undefined;



    let dataListItemContent = cloudRouterArray.map((obj, key) =>
        <option value={obj.address}></option>
        // <option value={obj.address}>{obj.address}</option>
    );

    return (
        <Container fluid>
           
            {showCDCSetup ? <div><h3>Cloud Router Setup</h3>















                <div id="modifyCloudRouter">

                    <Row>
                        {showCDCInput ?
                            <div className="py-3 col-6">
                                <input id="cloudRouterAddressRegister" onChange={(e) => { checkNewCloudRouter(e.target.value) }} autoComplete="new-address" placeholder="Enter Cloud Router Address" list="cloudRouterAddressRegisterElement" className="rounded col-8 text-13 form-control form-control-glow" type="text" name="routerAddress" />


                                <datalist id="cloudRouterAddressRegisterElement">{dataListItemContent}</datalist>
                            </div>
                            :
                            <div className="col-12">
                                {routerConfigCDC}
                                </div>
                        }
                        
                    </Row >









                    {showPortSetup ?
                        <div>

                            <h4 className=" mt-3 mb-0">Port Setup:</h4>
                            <Row>
                                <div className="col-md-3">
                                    <div data-port-name="CAN0" className=" dataCardModifyCR card-data my-3 card">
                                        <ul className="listview flush transparent image-listview">
                                            <li>
                                                <div className="p-1 item">
                                                    <div className="icon-box bg-white mr-1"><i className="text-primary fas fa-ethernet"></i></div>
                                                    <div className="in"><span data-portname="CAN0" className="portNameModify text-right text-bold">Smart1</span></div>
                                                </div>
                                            </li>
                                            <li>
                                                <div className="p-1 item">
                                                    <div className="icon-box bg-white mr-1"><i className="text-yellow fas fa-bolt"></i></div>
                                                    <div className="in">
                                                        <div>Rapid Read</div>
                                                        <div className="custom-control custom-switch">
                                                            <input defaultChecked={checkedSmart1Batch} onChange={(e) => portsChecked(e)} type="checkbox" className="inputParCR0 parChargeModeInput custom-control-input" data-port="smart1" id="smart1RapidRead" /> <label className="custom-control-label custom-control-label-on" htmlFor="smart1RapidRead"></label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </li>
                                            <li>
                                                <div className="p-1 item">
                                                    <div className="icon-box bg-white mr-1"><i className="text-warning fas fa-bolt"></i></div>
                                                    <div className="in">
                                                        <div>Par Charge</div>
                                                        <div className="custom-control custom-switch">
                                                            <input onChange={(e) => portsChecked(e)} type="checkbox" className="inputParCR0 parBatchModeInput custom-control-input" data-port="smart1" id="smart1BatchMode" /> <label className="custom-control-label custom-control-label-on" htmlFor="smart1BatchMode"></label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <div className="col-md-3">
                                    <div data-port-name="CAN1" className=" dataCardModifyCR card-data my-3 card">
                                        <ul className="listview flush transparent image-listview">
                                            <li>
                                                <div className="p-1 item">
                                                    <div className="icon-box bg-white mr-1"><i className="text-primary fas fa-ethernet"></i></div>
                                                    <div className="in"><span data-portname="CAN1" className="portNameModify text-right text-bold">Smart2</span></div>
                                                </div>
                                            </li>
                                            <li>
                                                <div className="p-1 item">
                                                    <div className="icon-box bg-white mr-1"><i className="text-yellow fas fa-bolt"></i></div>
                                                    <div className="in">
                                                        <div>Rapid Read</div>
                                                        <div className="custom-control custom-switch">
                                                            <input onChange={(e) => portsChecked(e)} type="checkbox" className="inputParCR1 parChargeModeInput custom-control-input" data-port="smart2" id="smart2RapidRead" /> <label className="custom-control-label custom-control-label-on" htmlFor="smart2RapidRead"></label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </li>
                                            <li>
                                                <div className="p-1 item">
                                                    <div className="icon-box bg-white mr-1"><i className="text-warning fas fa-bolt"></i></div>
                                                    <div className="in">
                                                        <div>Par Charge</div>
                                                        <div className="custom-control custom-switch">
                                                            <input onChange={(e) => portsChecked(e)} type="checkbox" className="inputParCR1 parBatchModeInput custom-control-input" data-port="smart2" id="smart2BatchMode" /> <label className="custom-control-label custom-control-label-on" htmlFor="smart2BatchMode"></label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <div className="col-md-3">
                                    <div data-port-name="COW_A" className=" dataCardModifyCR card-data my-3 card">
                                        <ul className="listview flush transparent image-listview">
                                            <li>
                                                <div className="p-1 item">
                                                    <div className="icon-box bg-white mr-1"><i className="text-primary fas fa-ethernet"></i></div>
                                                    <div className="in"><span className="portNameModify text-right text-bold">ClassicA</span></div>
                                                </div>
                                            </li>
                                            <li>
                                                <div className="p-1 item">
                                                    <div className="icon-box bg-white mr-1"><i className="text-yellow fas fa-bolt"></i></div>
                                                    <div className="in">
                                                        <div>Rapid Read</div>
                                                        <div className="custom-control custom-switch">
                                                            <input onChange={(e) => portsChecked(e)} type="checkbox" className="inputParCR2 parChargeModeInput custom-control-input" data-port="classicA" id="classicARapidRead" /> <label className="custom-control-label custom-control-label-on" htmlFor="classicARapidRead"></label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </li>
                                            <li>
                                                <div className="p-1 item">
                                                    <div className="icon-box bg-white mr-1"><i className="text-warning fas fa-bolt"></i></div>
                                                    <div className="in">
                                                        <div>Par Charge</div>
                                                        <div className="custom-control custom-switch">
                                                            <input onChange={(e) => portsChecked(e)} type="checkbox" className="inputParCR2 parBatchModeInput custom-control-input" data-port="classicA" id="classicABatchMode" /> <label className="custom-control-label custom-control-label-on" htmlFor="classicABatchMode"></label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <div className="col-md-3">
                                    <div data-port-name="COW_B" className=" dataCardModifyCR card-data my-3 card">
                                        <ul className="listview flush transparent image-listview">
                                            <li>
                                                <div className="p-1 item">
                                                    <div className="icon-box bg-white mr-1"><i className="text-primary fas fa-ethernet"></i></div>
                                                    <div className="in"><span className="portNameModify text-right text-bold">ClassicB</span></div>
                                                </div>
                                            </li>
                                            <li>
                                                <div className="p-1 item">
                                                    <div className="icon-box bg-white mr-1"><i className="text-yellow fas fa-bolt"></i></div>
                                                    <div className="in">
                                                        <div>Rapid Read</div>
                                                        <div className="custom-control custom-switch">
                                                            <input onChange={(e) => portsChecked(e)} type="checkbox" className="inputParCR3 parChargeModeInput custom-control-input" data-port="classicB" id="classicBRapidRead" /> <label className="custom-control-label custom-control-label-on" htmlFor="classicBRapidRead"></label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </li>
                                            <li>
                                                <div className="p-1 item">
                                                    <div className="icon-box bg-white mr-1"><i className="text-warning fas fa-bolt"></i></div>
                                                    <div className="in">
                                                        <div>Par Charge</div>
                                                        <div className="custom-control custom-switch">
                                                            <input onChange={(e) => portsChecked(e)} type="checkbox" className="inputParCR3 parBatchModeInput custom-control-input" data-port="classicB" id="classicBBatchMode" /> <label className="custom-control-label custom-control-label-on" htmlFor="classicBBatchMode"></label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                            </Row>




                            <Row>
                                <div className="col-md-3">
                                    <div data-port-name="COW_C" className="dataCardModifyCR card-data my-3 card">
                                        <ul className="listview flush transparent image-listview">
                                            <li>
                                                <div className="p-1 item">
                                                    <div className="icon-box bg-white mr-1"><i className="text-primary fas fa-ethernet"></i></div>
                                                    <div className="in"><span className="portNameModify text-right text-bold">ClassicC</span></div>
                                                </div>
                                            </li>
                                            <li>
                                                <div className="p-1 item">
                                                    <div className="icon-box bg-white mr-1"><i className="text-yellow fas fa-bolt"></i></div>
                                                    <div className="in">
                                                        <div>Rapid Read</div>
                                                        <div className="custom-control custom-switch">
                                                            <input onChange={(e) => portsChecked(e)} type="checkbox" className="inputParCR4 parChargeModeInput custom-control-input" data-port="classicC" id="classicCRapidRead" /> <label className="custom-control-label custom-control-label-on" htmlFor="classicCRapidRead"></label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </li>
                                            <li>
                                                <div className="p-1 item">
                                                    <div className="icon-box bg-white mr-1"><i className="text-warning fas fa-bolt"></i></div>
                                                    <div className="in">
                                                        <div>Par Charge</div>
                                                        <div className="custom-control custom-switch">
                                                            <input onChange={(e) => portsChecked(e)} type="checkbox" className="inputParCR4 parBatchModeInput custom-control-input" data-port="classicC" id="classicCBatchMode" /> <label className="custom-control-label custom-control-label-on" htmlFor="classicCBatchMode"></label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <div className="col-md-3">
                                    <div className="dataCardModifyCR card-data my-3 card">
                                        <ul className="listview flush transparent image-listview">
                                            <li>
                                                <div className="p-1 item">
                                                    <div className="icon-box bg-white mr-1"><i className="text-primary fas fa-ethernet"></i></div>
                                                    <div className="in"><span className="portNameModify text-right text-bold">ClassicD</span></div>
                                                </div>
                                            </li>
                                            <li>
                                                <div className="p-1 item">
                                                    <div className="icon-box bg-white mr-1"><i className="text-yellow fas fa-bolt"></i></div>
                                                    <div className="in">
                                                        <div>Rapid Read</div>
                                                        <div className="custom-control custom-switch">
                                                            <input onChange={(e) => portsChecked(e)} type="checkbox" className="inputParCR5 parChargeModeInput custom-control-input" data-port="classicD" id="classicDRapidRead" /> <label className="custom-control-label custom-control-label-on" htmlFor="classicDRapidRead"></label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </li>
                                            <li>
                                                <div className="p-1 item">
                                                    <div className="icon-box bg-white mr-1"><i className="text-warning fas fa-bolt"></i></div>
                                                    <div className="in">
                                                        <div>Par Charge</div>
                                                        <div className="custom-control custom-switch">
                                                            <input onChange={(e) => portsChecked(e)} type="checkbox" className="inputParCR5 parBatchModeInput custom-control-input" data-port="classicD" id="classicDBatchMode" /> <label className="custom-control-label custom-control-label-on" htmlFor="classicDBatchMode"></label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </li>
                                        </ul>
                                    </div>

                                </div>
                                <div className="col-md-3">
                                    <div className="dataCardModifyCR card-data my-3 card">
                                        <ul className="listview flush transparent image-listview">
                                            <li>
                                                <div className="p-1 item">
                                                    <div className="icon-box bg-white mr-1"><i className="text-primary fas fa-ethernet"></i></div>
                                                    <div className="in"><span className="portNameModify text-right text-bold">ClassicE</span></div>
                                                </div>
                                            </li>
                                            <li>
                                                <div className="p-1 item">
                                                    <div className="icon-box bg-white mr-1"><i className="text-yellow fas fa-bolt"></i></div>
                                                    <div className="in">
                                                        <div>Rapid Read</div>
                                                        <div className="custom-control custom-switch">
                                                            <input onChange={(e) => portsChecked(e)} type="checkbox" className="inputParCR6 parChargeModeInput custom-control-input" data-port="classicE" id="classicERapidRead" /> <label className="custom-control-label custom-control-label-on" htmlFor="classicERapidRead"></label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </li>
                                            <li>
                                                <div className="p-1 item">
                                                    <div className="icon-box bg-white mr-1"><i className="text-warning fas fa-bolt"></i></div>
                                                    <div className="in">
                                                        <div>Par Charge</div>
                                                        <div className="custom-control custom-switch">
                                                            <input onChange={(e) => portsChecked(e)} type="checkbox" className="inputParCR6 parBatchModeInput custom-control-input" data-port="classicE" id="classicEBatchMode" /> <label className="custom-control-label custom-control-label-on" htmlFor="classicEBatchMode"></label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <div className="col-md-3">
                                    <div className="dataCardModifyCR card-data my-3 card">
                                        <ul className="listview flush transparent image-listview">
                                            <li>
                                                <div className="p-1 item">
                                                    <div className="icon-box bg-white mr-1"><i className="text-primary fas fa-ethernet"></i></div>
                                                    <div className="in"><span className="portNameModify text-right text-bold">ClassicF</span></div>
                                                </div>
                                            </li>
                                            <li>
                                                <div className="p-1 item">
                                                    <div className="icon-box bg-white mr-1"><i className="text-yellow fas fa-bolt"></i></div>
                                                    <div className="in">
                                                        <div>Rapid Read</div>
                                                        <div className="custom-control custom-switch">
                                                            <input onChange={(e) => portsChecked(e)} type="checkbox" className="inputParCR7 parChargeModeInput custom-control-input" data-port="classicF" id="classicFRapidRead" /> <label className="custom-control-label custom-control-label-on" htmlFor="classicFRapidRead"></label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </li>
                                            <li>
                                                <div className="p-1 item">
                                                    <div className="icon-box bg-white mr-1"><i className="text-warning fas fa-bolt"></i></div>
                                                    <div className="in">
                                                        <div>Par Charge</div>
                                                        <div className="custom-control custom-switch">
                                                            <input onChange={(e) => portsChecked(e)} type="checkbox" className="inputParCR7 parBatchModeInput custom-control-input" data-port="classicF" id="classicFBatchMode" /> <label className="custom-control-label custom-control-label-on" htmlFor="classicFBatchMode"></label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                            </Row>

                            <Row className="justify-content-center"> <div onClick={() => registerNewCloudRouter()} className="mt-4 btn btn-md btn-primary">Create CDC Router</div></Row>
                        </div>
                        : null}
                </div>

            </div> : null
                }

            {loaderCDC}
            {loaderCDCSuccess}
            {loaderCDCError}
        </Container>
    );
}

export default ConfigureCloudRouterDevice;