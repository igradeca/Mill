using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mill.Engine {
    public class Tween {

        private double _original;
        private double _distance;
        private double _current;
        private double _totalTimePassed = 0;
        private double _totalDuration = 5;
        private bool _finished = false;

        TweenFunction _tweenFunction = null;
        public delegate double TweenFunction(double timePassed, double start, double distance, double duration);

        public double Value() {

            return _current;
        }

        public bool IsFinished() {

            return _finished;
        }

        public static double Linear(double timePassed, double start, double distance, double duration) {

            return distance * timePassed / duration + start;
        }

        public Tween(double start, double end, double time) {

            Construct(start, end, time, Tween.Linear);
        }

        public Tween(double start, double end, double time, TweenFunction tweenFunction) {

            Construct(start, end, time, tweenFunction);
        }

        public void Construct(double start, double end, double time, TweenFunction tweenFunction) {

            _distance = end - start;
            _original = start;
            _current = start;
            _totalDuration = time;
            _tweenFunction = tweenFunction;
        }

        public void Update(double elapsedTime) {

            _totalTimePassed += elapsedTime;
            _current = _tweenFunction(_totalTimePassed, _original, _distance, _totalDuration);

            if (_totalTimePassed > _totalDuration) {
                _current = _original + _distance;
                _finished = true;
            }
        }


    }
}
