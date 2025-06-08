import React from "react";
import POMODORO1 from "./POMODORO-1.png";
import WORKSPACE from "./WORKSPACE.png";
import group3 from "./group-3.png";
import rectangle1 from "./rectangle-1.svg";
import "./style.css";
import tIXuNg2 from "./t-i-xu-ng-2.png";

export const Home = () => {
    return (
        <div className="HOME">
            <div className="overlap-wrapper">
                <div className="overlap">
                    <img className="ti-xung" alt="Ti xung" src={tIXuNg2} />

                    <div className="group">
                        <div className="overlap-group">
                            <img className="rectangle" alt="Rectangle" src={rectangle1} />
                        </div>
                    </div>

                    <img className="POMODORO" alt="Pomodoro" src={POMODORO1} />

                    <div className="overlap-group-wrapper">
                        <div className="div">
                            <img className="img" alt="Group" src={group3} />

                            <div className="group-2" />
                        </div>
                    </div>

                    <div className="text-wrapper">HOME</div>

                    <div className="text-wrapper-2">MANAGEMENT</div>

                    <div className="group-3">
                        <div className="text-wrapper-3">WELCOME TO POMODORO!</div>

                        <p className="p">Digital workspace that help you focus.</p>

                        <p className="text-wrapper-4">
                            Đồ án môn Công nghệ Web - Nhóm 4 - 22CDP
                        </p>
                    </div>

                    <img className="WORKSPACE" alt="Workspace" src={WORKSPACE} />
                </div>
            </div>
        </div>
    );
};
