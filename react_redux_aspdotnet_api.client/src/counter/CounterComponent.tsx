import { useSelector, useDispatch } from 'react-redux';
import { RootState } from '../store';
import { increase, decrease, reset } from '../reducers/CounterReducer';

const CounterComponent = () => {
    const counter = useSelector((state: RootState) => state.counter.counter);
    const dispatch = useDispatch();

    const handleIncrease = () => {
        dispatch(increase());
    };

    const handleDecrease = () => {
        dispatch(decrease());
    };

    const handleReset = () => {
        dispatch(reset());
    };

    return (
        <div>
            <h1>Counter: {counter}</h1>
            <div className={""}>
                <button className={"btn btn-success me-1"} onClick={handleIncrease}>Increase</button>
                <button className={"btn btn-danger me-1"} onClick={handleDecrease}>Decrease</button>
                <button className={"btn btn-primary me-1"} onClick={handleReset}>Reset</button>
            </div>
        </div>
    );
};

export default CounterComponent;
