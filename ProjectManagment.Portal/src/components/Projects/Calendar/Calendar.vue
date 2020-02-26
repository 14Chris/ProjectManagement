<template>
  <div class="div-calendar-component" id="outer">
    <!-- <div id="inner_fixed">
      <h1>Calendar</h1>
    </div>

    <div class="div-calendar" id="inner_remaining"> -->
      <FullCalendar height="auto" defaultView="timeGridWeek" :plugins="calendarPlugins" />
      <!-- I take up the remaining height -->
    <!-- </div> -->
  </div>
</template>

<script>
import FullCalendar from "@fullcalendar/vue";
import dayGridPlugin from "@fullcalendar/daygrid";
import timeGridPlugin from "@fullcalendar/timegrid";

export default {
  name: "ProjectCalendar",
  components: {
    FullCalendar // make the <FullCalendar> tag available
  },
  data() {
    return {
      calendarPlugins: [dayGridPlugin, timeGridPlugin],
      events: []
    };
  },
  methods: {
    AddEvent() {}
  }
};
</script>

<style lang='scss' scoped>
@import "~@fullcalendar/core/main.css";
@import "~@fullcalendar/daygrid/main.css";
@import "~@fullcalendar/timegrid/main.css";

// .div-calendar-component {
//   display: flex;
//   height: 100%;
//   flex-flow: column;
// }

// .div-calendar {
//   flex: 1;
//   background-color: #dddddd;
//   overflow-y: scroll;
// }
#outer {
  display: flex;
  flex-flow: column;
  height: 100%;
}

#inner_fixed {
  height: 100px;
  background-color: grey;
}

#inner_remaining {
  background-color: #dddddd;
  flex: 1;
}
</style>