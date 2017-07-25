<template>
    <div class="container">
        <swiper height="280px" :show-dots="false" @on-index-change="changeSwiper" v-model="currentIndex">
            <swiper-item v-for="(item, index) in list" :key="index">
                <div class="index-content">
                    <img class="image" :src="item.src">
                    <div class="text">
                        {{item.text}}
                        <span v-show = "currentIndex >= 5 && currentIndex <= 9" class = "tip">请佩戴AR眼镜</span>
                    </div>
                    <div style="clear:both"></div>
                </div>
            </swiper-item>
        </swiper>
        <div class="navbar-fixed-bottom">
            <div class="row">
                <div class="col-xs-2 text-center">
                    <button v-show="currentIndex > 0" type="button" class="btn btn-default btn-sm" @click="prePage()">
                        上一步
                    </button>
                </div>
                <div class="col-xs-8">
                    <div class="progress" style="margin-top: 5px;">
                        <div class="progress-bar active" role="progressbar" :aria-valuenow="currentIndex + 1" aria-valuemin="1" :aria-valuemax="list.length" v-bind:style="{ width: styleWidth + '%' }">
                            第{{currentIndex+1}}步 共{{list.length}}步
                        </div>
                    </div>
                </div>
                <div class="col-xs-2 text-center">
                    <button v-show="currentIndex < list.length-1" type="button" class="btn btn-default btn-sm" @click="nextPage()">
                        下一步
                    </button>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
import { Swiper, SwiperItem } from 'vux'

export default {
    components: {
        Swiper,
        SwiperItem
    },
    data () {
        return {
            currentIndex: 0,
            list: [
                { src: require('../assets/step1.png'), text: '1.佩戴面罩' },
                { src: require('../assets/step2.png'), text: '2.戴好防护手套' },
                { src: require('../assets/step3.png'), text: '3.确认车辆（车牌、危险标志）' },
                { src: require('../assets/step4.png'), text: '4.检查登记驾驶员押运员资质' },
                { src: require('../assets/step5.png'), text: '5.场安全隔离' },
                { src: require('../assets/step6.png'), text: '6.确认所进药剂与连接管道一致药剂管道' },
                { src: require('../assets/step7.png'), text: '7.打开进药口阀门开始进药' },
                { src: require('../assets/step8.png'), text: '8.进药全过程监护' },
                { src: require('../assets/step9.png'), text: '9.进完药剂关闭进药口阀门' },
                { src: require('../assets/step10.png'), text: '10.脱开进药管后，封好盲板' },
                { src: require('../assets/step11.png'), text: '11.进药现场进行冲洗' },
                { src: require('../assets/step12.png'), text: '12.药剂磅单登记' }
            ]
        }
    },
    computed: {
        styleWidth: function () {
            return Number((this.currentIndex + 1) / this.list.length * 100)
        }
    },
    methods: {
        changeSwiper (index) {
            this.currentIndex = index
        },
        prePage () {
            if (this.currentIndex > 0) {
                this.currentIndex--
            }
        },
        nextPage () {
            if (this.currentIndex < this.list.length - 1) {
                this.currentIndex++
            }
        }
    }
}
</script>
<style lang = "scss" scoped>
.index-content {
    width: 100%;
    margin-top: 10px;
}

.swiper {
    height: 280px !important;
}

.image {
    float: left;
    height: 280px;
    width: 60%;
}

.text {
    float: left;
    height: 280px;
    padding-left: 15px;
    width: 30%;
    font-size: 18px;
}
.tip{
    display: block;
    color: red;
    font-size: 18px;
}

@media screen and (max-height: 320px) {
    .image {
        height: 240px;
    }
    .text {
        height: 240px;
    }
    .swiper {
        height: 240px !important;
    }
}

@media screen and (min-height: 414px) {
    .image {
        height: 320px;
    }
    .text {
        height: 320px;
    }
    .swiper {
        height: 320px !important;
    }
}
</style>
