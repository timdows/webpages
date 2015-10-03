<?php
namespace BackendBundle\ClientModels;

class SevenSegment
{
    public $watt;
    
    public $lastWeekStartLow;
    public $lastWeekStopLow;
    public $lastWeekLow;
    public $lastWeekStartHigh;
    public $lastWeekStopHigh;
    public $lastWeekHigh;
    public $lastWeekTotal;
    
    public $thisWeekStartLow;
    public $thisWeekStopLow;
    public $thisWeekLow;
    public $thisWeekStartHigh;
    public $thisWeekStopHigh;
    public $thisWeekHigh;
    public $thisWeekTotal;
    
    public $lastMonthStartLow;
    public $lastMonthStopLow;
    public $lastMonthLow;
    public $lastMonthStartHigh;
    public $lastMonthStopHigh;
    public $lastMonthHigh;
    public $lastMonthTotal;
    
    public $thisMonthStartLow;
    public $thisMonthStopLow;
    public $thisMonthLow;
    public $thisMonthStartHigh;
    public $thisMonthStopHigh;
    public $thisMonthHigh;
    public $thisMonthTotal;
    
 /**
     * @return the $watt
     */
    public function getWatt()
    {
        return $this->watt;
    }

 /**
     * @return the $lastWeekTotal
     */
    public function getLastWeekTotal()
    {
        return $this->lastWeekTotal;
    }

 /**
     * @return the $thisWeekTotal
     */
    public function getThisWeekTotal()
    {
        return $this->thisWeekTotal;
    }

 /**
     * @param field_type $watt
     */
    public function setWatt($watt)
    {
        $this->watt = $watt;
    }

 /**
     * @param field_type $lastWeekTotal
     */
    public function setLastWeekTotal($lastWeekTotal)
    {
        $this->lastWeekTotal = $lastWeekTotal;
    }

 /**
     * @param field_type $thisWeekTotal
     */
    public function setThisWeekTotal($thisWeekTotal)
    {
        $this->thisWeekTotal = $thisWeekTotal;
    }
    
 /**
     * @return the $lastMonthTotal
     */
    public function getLastMonthTotal()
    {
        return $this->lastMonthTotal;
    }

 /**
     * @return the $thisMonthTotal
     */
    public function getThisMonthTotal()
    {
        return $this->thisMonthTotal;
    }

 /**
     * @param field_type $lastMonthTotal
     */
    public function setLastMonthTotal($lastMonthTotal)
    {
        $this->lastMonthTotal = $lastMonthTotal;
    }

 /**
     * @param field_type $thisMonthTotal
     */
    public function setThisMonthTotal($thisMonthTotal)
    {
        $this->thisMonthTotal = $thisMonthTotal;
    }


    
    
    
}